using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Models;
using ESG.API.DTOs;

namespace ESG.Api.Repository
{
    public class LoanApplicationRepo : ILoanApplicationRepo
    {
        private AppDbContext _context;
        public LoanApplicationRepo(AppDbContext context)
        {
            _context = context;
        }
        public bool CreateLoanApplication(LoanApplicationForCreationDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);
            
            var loanApplication = new LOAN_APPLICATION {
                PRODUCTID = model.productId,
                AMOUNT = model.amount,
                TENORINDAYS = model.tenorInDays,
                INTERESTRATE = model.interestRate,
                LOANPURPOSE = model.loanPurpose,
                APPLICATIONDATE = DateTime.Now
            };

            _context.LOAN_APPLICATION.Add(loanApplication);
            return SaveChanges();
        }

        public bool DeleteLoanApplication(int id)
        {
            bool output = false;
            var loanApplication = _context.LOAN_APPLICATION.Find(id);
            if (loanApplication != null)
            {
                _context.LOAN_APPLICATION.Remove(loanApplication);
                output = SaveChanges();
                return output;
            }

            return output;
        }

        public List<LoanApplicationForCreationDTO> GetAllLoanApplication()
        {
            var loanApplications = _context.LOAN_APPLICATION.Select(x => new LoanApplicationForCreationDTO
            {
                
            }).ToList();

            return loanApplications;
        }

        public LoanApplicationForCreationDTO GetLoanApplicationById(int id)
        {
            var loanApplication = _context.LOAN_APPLICATION.Where(x => x.LOANAPPLICATIONID == id).Select(x => new LoanApplicationForCreationDTO
            {
                
            }).FirstOrDefault();

            return loanApplication ?? new LoanApplicationForCreationDTO();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public bool UpdateLoanApplication(LoanApplicationForCreationDTO loanApplication)
        {
            throw new NotImplementedException();
        }
    }
}