using ESG.Api.Data;
using ESG.Api.DTos;
using ESG.Api.Interface;
using ESG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ESG.Api.Repository
{
    public class EsgAiRecommendationRepo : IEsgAiRecommendationRepo
    {
        private readonly AppDbContext _context;

        public EsgAiRecommendationRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(ESG_AI_RECOMMENDATION recommendation)
        {
            _context.ESG_AI_RECOMMENDATION.Add(recommendation);
            await _context.SaveChangesAsync();
        }

        public async Task<ESG_AI_RECOMMENDATION?> GetEsgScreeningRecommendationAsync(long loanApplicationId, string stage)
        {
            var recommendation = await _context.ESG_AI_RECOMMENDATION
                .Where(x => x.LOANAPPLICATIONID == loanApplicationId && x.STAGE == stage)
                .Select(x => new ESG_AI_RECOMMENDATION
                {
                    ID = x.ID,
                    LOANAPPLICATIONID = x.LOANAPPLICATIONID,
                    STAGE = x.STAGE,
                    RISKLEVEL = x.RISKLEVEL,
                    RECOMMENDATION = x.RECOMMENDATION,
                    CONFIDENCE = x.CONFIDENCE,
                    PAYLOAD = x.PAYLOAD,
                    MODELVERSION = x.MODELVERSION,
                    CREATEDBY = x.CREATEDBY,
                    DATETIMECREATED = x.DATETIMECREATED
                }).OrderByDescending(x => x.ID).FirstOrDefaultAsync();

            return recommendation;
        }

        public async Task UpdateAsync(ESG_AI_RECOMMENDATION recommendation)
        {
            _context.ESG_AI_RECOMMENDATION.Update(recommendation);
            await _context.SaveChangesAsync();
        }

        public Task<EsgChecklistSummaryDto?> GetEsgAssessmentSummaryLiteAsync(long loanApplicationId)
        {
            var summary = _context.ESG_CHECKLIST_SUMMARY
                .Where(x => x.LOANAPPLICATIONID == loanApplicationId)
                .Select(x => new EsgChecklistSummaryDto
                {
                    ratingId = x.RATINGID,
                    averageScore = x.AVGSCORE,
                    confidence = x.CONFIDENCE,
                }).FirstOrDefaultAsync();

            return summary;
        }
    }
}