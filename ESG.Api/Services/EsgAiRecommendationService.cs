using System.Text.Json;
using ESG.Api.Contracts;
using ESG.Api.DTOs;
using ESG.Api.Interface;
using ESG.Api.Models;
using ESGG.Api.Enums;

namespace ESG.Api.Services
{
    public class EsgAiRecommendationService : IEsgAiRecommendationService
    {
        private readonly IEsgAiRecommendationRepo _repo;

        public EsgAiRecommendationService(IEsgAiRecommendationRepo repo)
        {
            _repo = repo;
        }

        public async Task<EsgAiRecommendationDTO> PreScreenAsync(PreScreenRequest request)
        {
            var riskLevelId = request.sectorId == 2
                ? (int)EsgAiRiskLevelEnum.High
                : (int)EsgAiRiskLevelEnum.Medium;

            var recommendation = new EsgAiRecommendationDTO
            {
                LoanApplicationId = request.loanApplicationId,
                Stage = "PRE_SCREEN",
                RiskLevel = (short)riskLevelId,
                Recommendation = riskLevelId == (int)EsgAiRiskLevelEnum.High
                    ? "Enhanced Due Diligence And Mitigation Measures Required"
                    : "Proceed With Conditions And Secondary Review",
                Confidence = 0.85m,
                Payload = JsonSerializer.Serialize(request)
            };

            var preScreenRecommendation = new ESG_AI_RECOMMENDATION
            {
                LOANAPPLICATIONID = recommendation.LoanApplicationId,
                STAGE = "PRE_SCREEN",
                RISKLEVEL = (short)recommendation.RiskLevel,
                RECOMMENDATION = recommendation.Recommendation,
                CONFIDENCE = recommendation.Confidence,
                PAYLOAD = recommendation.Payload,
            };

            await _repo.SaveAsync(preScreenRecommendation);
            return recommendation;
        }

        public async Task<EsgAiRecommendationDTO> FinalRecommendationAsync(FinalAssessmentRequest request)
        {
            var riskLevelId = request.averageScore >= 65
                ? (int)EsgAiRiskLevelEnum.High
                : request.averageScore >= 35
                    ? (int)EsgAiRiskLevelEnum.Medium
                    : (int)EsgAiRiskLevelEnum.Low;

            var recommendation = request.averageScore switch
            {
                >= 65 => EsgAiRecommendationEnum.EscalateForComprehensiveEsgReview,
                >= 45 => EsgAiRecommendationEnum.EnhancedDueDiligenceAndMitigationRequired,
                >= 30 => EsgAiRecommendationEnum.ProceedWithConditionsAndSecondaryReview,
                _ => EsgAiRecommendationEnum.ProceedProvisionally
            };

            var entity = new EsgAiRecommendationDTO
            {
                LoanApplicationId = request.loanApplicationId,
                Stage = "FINAL",
                RiskLevel = (short)riskLevelId,
                Recommendation = recommendation.ToString(),
                Confidence = 0.92m,
                Payload = JsonSerializer.Serialize(request)
            };

            var finalRecommendation = new ESG_AI_RECOMMENDATION
            {
                LOANAPPLICATIONID = entity.LoanApplicationId,
                STAGE = "FINAL",
                RISKLEVEL = (short)entity.RiskLevel,
                RECOMMENDATION = entity.Recommendation,
                CONFIDENCE = entity.Confidence,
                PAYLOAD = entity.Payload,
            };

            await _repo.SaveAsync(finalRecommendation);
            return entity;
        }
    }
}