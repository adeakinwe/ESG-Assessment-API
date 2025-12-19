using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IEsgMlFeatureService
    {
        Task<EsgMlFeatureDTO> ExtractFeaturesAsync(int loanApplicationId);
    }
}