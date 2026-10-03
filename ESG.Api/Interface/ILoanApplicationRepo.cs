using ESG.API.DTOs;

namespace ESG.Api.Interface 
{
    public interface ILoanApplicationRepo
    {
        List<LoanApplicationForReturnDTO> GetAllLoanApplication();
        LoanApplicationForReturnDTO GetLoanApplicationById(int id);
        string CreateLoanApplication(LoanApplicationForCreationDTO model);
        Task<bool> UpdateLoanApplication(LoanApplicationForCreationDTO loanApplication, int id);
        bool DeleteLoanApplication(int id);
        bool SaveChanges();
        Task<bool> SubmitLoanApplicationForAppraisalAsync(int id);
    }
}