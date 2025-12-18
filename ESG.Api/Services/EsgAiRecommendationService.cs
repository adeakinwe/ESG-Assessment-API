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
        private readonly IEsgExplainabilityService _explainabilityService;

        public EsgAiRecommendationService(IEsgAiRecommendationRepo repo, IEsgExplainabilityService explainabilityService)
        {
            _repo = repo;
            _explainabilityService = explainabilityService;
        }

        private decimal ComputeConfidence(PreScreenRequest request)
        {
            decimal confidence = 0.60m;

            if (request.sectorId == 2) confidence += 0.15m;

            if (request.loanAmount > 500000000) confidence += 0.10m;

            if (request.country == "NG") confidence += 0.05m;

            return Math.Round(Math.Min(confidence, 0.95m), 2);
        }

        private string GetSectorName(int sectorId)
        {
            return sectorId switch
            {
                1 => "Agriculture",
                2 => "Manufacturing",
                3 => "Services",
                _ => "Unknown"
            };
        }

        private string GetProductName(int productId)
        {
            return productId switch
            {
                1 => "Overdraft",
                2 => "Term Loan",
                3 => "Invoice Discounting",
                4 => "Contingent",
                _ => "Others'"
            };
        }
        public async Task<EsgAiRecommendationDTO> PreScreenAsync(PreScreenRequest request)
        {
            var riskLevelId = request.sectorId == 2
                ? (int)EsgAiRiskLevelEnum.High
                : (int)EsgAiRiskLevelEnum.Medium;

            var recommendationText = riskLevelId == (int)EsgAiRiskLevelEnum.High
                    ? "Enhanced Due Diligence And Mitigation Measures Required"
                    : "Proceed With Conditions And Secondary Review";

            var payload = JsonSerializer.Serialize(request);
            decimal computedConfidence = ComputeConfidence(request);

            // --- EXPLAINABILITY LOGIC ---
            var explainability = new List<ExplainabilityItem>();

            // Sector impact
            explainability.Add(new ExplainabilityItem
            {
                Factor = "Sector",
                Impact = request.sectorId == 2 ? "Negative" : "Positive",
                Detail = $"Sector = {GetSectorName(request.sectorId)} → {(request.sectorId == 2 ? "Higher ESG risk" : "Lower ESG risk")}"
            });

            // Loan Amount impact
            explainability.Add(new ExplainabilityItem
            {
                Factor = "Loan Amount",
                Impact = request.loanAmount > 1000000 ? "Neutral" : "Positive",
                Detail = $"Loan Amount = {request.loanAmount:C} → {(request.loanAmount > 1000000 ? "Requires additional review" : "Within normal threshold")}"
            });

            // Product impact
            explainability.Add(new ExplainabilityItem
            {
                Factor = "Product",
                Impact = "Neutral",
                Detail = $"Product ID = {GetProductName(request.productId)} → Standard ESG review"
            });
            var recommendation = new EsgAiRecommendationDTO
            {
                LoanApplicationId = request.loanApplicationId,
                Stage = "PRE_SCREEN",
                RiskLevel = (short)riskLevelId,
                Recommendation = recommendationText,
                Confidence = computedConfidence,
                Payload = payload,
                Explainability = explainability
            };

            var existingPrescreen = await _repo.GetEsgScreeningRecommendationAsync(request.loanApplicationId, "PRE_SCREEN");
            if (existingPrescreen != null)
            {
                existingPrescreen.RISKLEVEL = recommendation.RiskLevel;
                existingPrescreen.RECOMMENDATION = recommendation.Recommendation;
                existingPrescreen.CONFIDENCE = recommendation.Confidence;
                existingPrescreen.PAYLOAD = recommendation.Payload;
                existingPrescreen.LASTUPDATEDBY = 1;
                existingPrescreen.DATETIMEUPDATED = DateTime.Now;

                await _repo.UpdateAsync(existingPrescreen);
            }
            else
            {
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
            }

            return recommendation;
        }

        public async Task<EsgFinalRecommendationDTO> GenerateFinalRecommendationAsync(int loanApplicationId)
        {
            // 1️⃣ Fetch all inputs
            var preScreen = await _repo.GetEsgScreeningRecommendationAsync(loanApplicationId, "PRE_SCREEN");
            var summary = await _repo.GetEsgAssessmentSummaryLiteAsync(loanApplicationId);
            var explainability = await _explainabilityService.GenerateExplainabilityAsync(loanApplicationId);

            // 2️⃣ Compute final risk (simplified weighted logic)
            short finalRisk = summary?.averageScore >= 65 || preScreen?.RISKLEVEL == (short)EsgAiRiskLevelEnum.High
                ? (short)EsgAiRiskLevelEnum.High
                : summary?.averageScore >= 35
                    ? (short)EsgAiRiskLevelEnum.Medium
                    : (short)EsgAiRiskLevelEnum.Low;

            // 3️⃣ Generate executive summary
            string summaryText = $"The loan application shows a {summary?.rating} ESG risk. " +
                                 $"{(explainability.Flags.Any() ? "High-risk areas identified: " + string.Join(", ", explainability.Flags) + ". " : "")}" +
                                 $"{(explainability.MitigationHints.Any() ? "Recommended mitigation: " + string.Join(", ", explainability.MitigationHints) + "." : "")}";

            // 4️⃣ Recommendation text
            string recommendation = finalRisk switch
            {
                (short)EsgAiRiskLevelEnum.High => "Enhanced Due Diligence and Mitigation Measures Required",
                (short)EsgAiRiskLevelEnum.Medium => "Proceed with Conditions and Secondary Review",
                _ => "Proceed with Standard Review"
            };

            // 5️⃣ Confidence computation (example: average of pre-screen & assessment confidence)
            decimal? confidence = Math.Round((preScreen.CONFIDENCE + summary.confidence) / 2, 2);

            return new EsgFinalRecommendationDTO
            {
                LoanApplicationId = loanApplicationId,
                Recommendation = recommendation,
                RiskLevel = finalRisk,
                Confidence = (decimal)confidence,
                Flags = explainability.Flags,
                MitigationHints = explainability.MitigationHints,
                SummaryText = summaryText
            };
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
                Payload = JsonSerializer.Serialize(request),
                Explainability = new List<ExplainabilityItem>()
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