using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using Microsoft.EntityFrameworkCore;
namespace ESG.Api.Services
{
    public class EsgMlFeatureService : IEsgMlFeatureService
    {
        private readonly AppDbContext _context;

        public EsgMlFeatureService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EsgMlFeatureDTO> ExtractFeaturesAsync(int loanApplicationId)
        {
            var assessments = await (
                from a in _context.ESG_CHECKLIST_ASSESSMENT
                join i in _context.ESG_CHECKLIST_ITEM
                    on a.CHECKLISTITEMID equals i.CHECKLISTITEMID
                where a.LOANAPPLICATIONID == loanApplicationId
                select new
                {
                    a.SCORE,
                    a.WEIGHT,
                    Category = i.CATEGORY
                }
            ).ToListAsync();

            if (!assessments.Any())
                throw new InvalidOperationException("No ESG assessment data found.");

            // Core aggregates
            var totalScore = assessments.Sum(x => x.SCORE * x.WEIGHT);
            var totalWeight = assessments.Sum(x => x.WEIGHT);
            decimal averageScore = totalWeight == 0 ? 0 : totalScore / totalWeight;

            // Normalized risk (0–100)
            const decimal maxScorePerItem = 10m;
            var normalizedRisk = (averageScore / maxScorePerItem) * 100;

            // Category breakdowns
            decimal CatAvg(string cat) =>
                assessments.Where(x => x.Category == cat).DefaultIfEmpty()
                    .Average(x => x == null ? 0 : (decimal)x.SCORE);

            decimal CatWeightRatio(string cat) =>
                totalWeight == 0 ? 0 :
                assessments.Where(x => x.Category == cat).Sum(x => x.WEIGHT) / totalWeight;

            // Distribution
            var scores = assessments.Select(x => x.SCORE).ToList();
            var mean = scores.Average();
            var variance = scores.Sum(s => (s - mean) * (s - mean)) / scores.Count;
            var stdDev = (decimal)Math.Sqrt((double)variance);

            // Explainability proxy (rule-aligned)
            int positive = assessments.Count(x => x.SCORE <= 3);
            int neutral = assessments.Count(x => x.SCORE > 3 && x.SCORE < 7);
            int negative = assessments.Count(x => x.SCORE >= 7);

            // Context (example – adapt to your schema)
            var sector = await (from a in _context.LOAN_APPLICATION
                              join b in _context.CUSTOMER
                                  on a.CUSTOMERID equals b.CUSTOMERID
                              where a.LOANAPPLICATIONID == loanApplicationId
                              select b.SECTOR).FirstOrDefaultAsync();

            return new EsgMlFeatureDTO
            {
                LoanApplicationId = loanApplicationId,

                AverageScore = Math.Round(averageScore, 2),
                TotalScore = totalScore,
                TotalWeight = totalWeight,
                NormalizedRiskScore = Math.Round(normalizedRisk, 2),

                EnvironmentAvgScore = CatAvg("Environment"),
                SocialAvgScore = CatAvg("Social"),
                GovernanceAvgScore = CatAvg("Governance"),

                EnvironmentWeightRatio = CatWeightRatio("Environment"),
                SocialWeightRatio = CatWeightRatio("Social"),
                GovernanceWeightRatio = CatWeightRatio("Governance"),

                ScoreStdDev = Math.Round(stdDev, 2),
                MaxScore = scores.Max(),
                MinScore = scores.Min(),

                HighRiskItemCount = assessments.Count(x => x.SCORE >= 8),
                LowRiskItemCount = assessments.Count(x => x.SCORE <= 2),

                PositiveImpactCount = positive,
                NeutralImpactCount = neutral,
                NegativeImpactCount = negative,

                SectorId = sector,
                CountryId = 1
            };
        }
    }
}