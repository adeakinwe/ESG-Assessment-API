using ESG.Api.Contracts;
using ESG.Api.DTOs;

namespace ESG.Api.Services
{
    public interface IEsgAiRecommendationService
    {
        Task<EsgAiRecommendationDTO> PreScreenAsync(PreScreenRequest request);
        Task<EsgAiRecommendationDTO> FinalRecommendationAsync(FinalAssessmentRequest request);
    }
}