using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IEsgMlSignalService
    {
        Task<EsgMlSignalDTO> EvaluateAsync(EsgMlFeatureDTO features);
    }
}