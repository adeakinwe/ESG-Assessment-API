using ESG.API.DTOs;

namespace ESG.Api.Interface 
{
    public interface ILoanApplicationRepo
    {
        List<LoanApplicationForReturnDTO> GetAllLoanApplication();
        LoanApplicationForReturnDTO GetLoanApplicationById(int id);
        string CreateLoanApplication(LoanApplicationForCreationDTO model);
        bool UpdateLoanApplication(LoanApplicationForCreationDTO loanApplication);
        bool DeleteLoanApplication(int id);
        bool SaveChanges();
        Task<bool> SubmitLoanApplicationForAppraisalAsync(int id);
    }
}