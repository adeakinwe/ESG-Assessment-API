using ESG.Api.Data;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using ESG.Api.Models;
using ESGG.Api.Enums;
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

        public async Task<EsgAiRecommendationDTO?> GetLatestAsync(long loanApplicationId, string stage)
        {
            var recommendation = await _context.ESG_AI_RECOMMENDATION
                .Where(x => x.LOANAPPLICATIONID == loanApplicationId && x.STAGE == stage)
                .Select(x => new EsgAiRecommendationDTO
                {
                    Id = x.ID,
                    LoanApplicationId = x.LOANAPPLICATIONID,
                    Stage = x.STAGE,
                    RiskLevel = x.RISKLEVEL,
                    Recommendation = x.RECOMMENDATION,
                    Confidence = x.CONFIDENCE,
                    Payload = x.PAYLOAD,
                    ModelVersion = x.MODELVERSION,
                    CreatedBy = x.CREATEDBY,
                    CreatedAt = x.DATETIMECREATED
                }).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

            return recommendation;
        }
    }
}