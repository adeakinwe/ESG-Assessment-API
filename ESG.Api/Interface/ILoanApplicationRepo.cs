using ESG.API.DTOs;

namespace ESG.Api.Interface 
{
    public interface ILoanApplicationRepo
    {
        List<LoanApplicationForCreationDTO> GetAllLoanApplication();
        LoanApplicationForCreationDTO GetLoanApplicationById(int id);
        bool CreateLoanApplication(LoanApplicationForCreationDTO model);
        bool UpdateLoanApplication(LoanApplicationForCreationDTO loanApplication);
        bool DeleteLoanApplication(int id);
        bool SaveChanges();
    }
}