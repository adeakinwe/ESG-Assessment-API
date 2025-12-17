using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IEsgExplainabilityService
    {
        Task<EsgExplainabilityDTO> GenerateExplainabilityAsync(int loanApplicationId);
    }
}