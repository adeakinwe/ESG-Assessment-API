using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Services
{
    public class EsgExplainabilityService : IEsgExplainabilityService
    {
        private readonly AppDbContext _context;

        public EsgExplainabilityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EsgExplainabilityDTO> GenerateExplainabilityAsync(int loanApplicationId)
        {
            var assessments = await (from a in _context.ESG_CHECKLIST_ASSESSMENT
                                     join b in _context.ESG_CHECKLIST_ITEM on a.CHECKLISTITEMID equals b.CHECKLISTITEMID
                                     where a.LOANAPPLICATIONID == loanApplicationId
                                     select new { a, b }
            ).ToListAsync();

            if (!assessments.Any())
                return new EsgExplainabilityDTO
                {
                    LoanApplicationId = loanApplicationId,
                    ItemDetails = new List<ChecklistExplainabilityItem>(),
                    Flags = new List<string>(),
                    MitigationHints = new List<string>()
                };

            var explainability = new EsgExplainabilityDTO
            {
                LoanApplicationId = loanApplicationId,
                ItemDetails = new List<ChecklistExplainabilityItem>(),
                Flags = new List<string>(),
                MitigationHints = new List<string>()
            };

            var mitigatedCategories = new List<string>();

            foreach (var item in assessments)
            {
                // Normalize contribution for consistent 0–100 scale
                double maxScore = 10; // adjust if your max SCORE differs
                double contribution = item.a.SCORE * item.a.WEIGHT;
                double normalizedContribution = contribution / (maxScore * item.a.WEIGHT) * 100;

                var category = item.b.CATEGORY?.ToUpper();

                // Determine impact (reverse because high score = high risk)
                string impact = normalizedContribution switch
                {
                    < 40 => "Positive",   // low risk
                    < 70 => "Neutral",    // medium risk
                    _ => "Negative"       // high risk
                };

                explainability.ItemDetails.Add(new ChecklistExplainabilityItem
                {
                    ChecklistItemId = item.a.CHECKLISTITEMID,
                    Question = item.b.CHECKLISTITEM ?? "Unknown",
                    Score = item.a.SCORE,
                    Weight = item.a.WEIGHT,
                    Contribution = normalizedContribution,
                    Impact = impact
                });

                // Flag high-risk items (high score = high risk)
                if (item.a.SCORE >= 8)
                {
                    explainability.Flags.Add(
                        $"{item.b.CATEGORY}: High risk response on \"{item.b.CHECKLISTITEM}\""
                    );
                    if (!string.IsNullOrEmpty(category) && !mitigatedCategories.Contains(category))
                    {
                        mitigatedCategories.Add(category);

                        explainability.MitigationHints.Add(category switch
                        {
                            "ENVIRONMENT" => "Environmental risk identified: emissions, waste management, or resource impact require mitigation",
                            "SOCIAL" => "Social risk identified: labor practices, health & safety, or community impact require mitigation",
                            "GOVERNANCE" => "Governance risk identified: compliance, controls, or oversight require strengthening",
                            _ => "Mitigation required for identified ESG risks"
                        });
                    }
                }
            }

            return explainability;
        }
    }
}