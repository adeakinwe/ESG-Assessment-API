using ESG.Api.DTos;
using ESG.Api.DTOs;

namespace ESG.Api.Interface
{
    public interface IChecklistRepo
    {
        Task<List<ChecklistItemDto>> GetChecklistAsync();
        Task<bool> IsLoanExistsAsync(int loanApplicationId);
        Task SubmitChecklistAssessmentAsync(EsgChecklistSubmissionDto entity);
    }
}