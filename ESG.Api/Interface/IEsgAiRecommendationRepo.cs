using ESG.Api.DTOs;
using ESG.Api.Models;

namespace ESG.Api.Interface
{
    public interface IEsgAiRecommendationRepo
    {
        Task SaveAsync(ESG_AI_RECOMMENDATION recommendation);
        Task<EsgAiRecommendationDTO?> GetLatestAsync(long loanApplicationId, string stage);
    }
}