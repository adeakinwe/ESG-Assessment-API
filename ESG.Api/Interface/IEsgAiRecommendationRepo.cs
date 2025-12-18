using ESG.Api.DTos;
using ESG.Api.DTOs;
using ESG.Api.Models;

namespace ESG.Api.Interface
{
    public interface IEsgAiRecommendationRepo
    {
        Task SaveAsync(ESG_AI_RECOMMENDATION recommendation);
        Task UpdateAsync(ESG_AI_RECOMMENDATION recommendation);
        Task<ESG_AI_RECOMMENDATION?> GetEsgScreeningRecommendationAsync(long loanApplicationId, string stage);
        Task<EsgChecklistSummaryDto?> GetEsgAssessmentSummaryLiteAsync(long loanApplicationId);
    }
}