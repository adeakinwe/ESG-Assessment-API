using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Models;
using ESG.API.DTOs;

namespace ESG.Api.Repository
{
    public class LoanApplicationRepo : ILoanApplicationRepo
    {
        private readonly AppDbContext _context;
        public LoanApplicationRepo(AppDbContext context)
        {
            _context = context;
        }
        public bool CreateLoanApplication(LoanApplicationForCreationDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var newLoanApplication = new LOAN_APPLICATION {
                CUSTOMERID = model.customerId,
                PRODUCTID = model.productId,
                AMOUNT = model.amount,
                TENOR = model.tenorInDays,
                INTERESTRATE = model.interestRate,
                LOANPURPOSE = model.loanPurpose,
                CURRENCYID = model.currencyId,
                APPROVALSTATUSID = 0, // Pending
                APPLICATIONDATE = DateTime.Now
            };

            _context.LOAN_APPLICATION.Add(newLoanApplication);
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

        public List<LoanApplicationForReturnDTO> GetAllLoanApplication()
        {
            var loanApplications = _context.LOAN_APPLICATION.Select(x => new LoanApplicationForReturnDTO
            {
                customerName = _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.FIRSTNAME + " " + c.LASTNAME).FirstOrDefault() ?? "",
                productName = x.PRODUCTID == 1 ? "Term Loan" : x.PRODUCTID == 2 ? "Overdraft" : "Others",
                amount = x.AMOUNT,
                tenorInDays = x.TENOR,
                interestRate = x.INTERESTRATE,
                loanPurpose = x.LOANPURPOSE,
                loanApplicationId = x.LOANAPPLICATIONID,
                sectorName = _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 1 ? "Agriculture" :
                             _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 2 ? "Manufacturing" :
                             _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 3 ? "Services" :
                             _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 4 ? "Trade" :
                             _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 5 ? "Oil and Gas" : "Others",
                currencyCode = x.CURRENCYID == 1 ? "NGN" : x.CURRENCYID == 2 ? "USD" : x.CURRENCYID == 3 ? "GBP" : x.CURRENCYID == 4 ? "EUR" : "Others" ,
                applicationDate = x.APPLICATIONDATE,
                approvalStatusId = x.APPROVALSTATUSID,
                statusName = _context.APPROVAL_STATUS.Where(a => a.APPROVALSTATUSID == x.APPROVALSTATUSID).Select(a => a.NAME).FirstOrDefault() ?? "Pending",
                riskRatingId = _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault(),
                riskRating = _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault() == 3 ? "High"
                             : _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault() == 2 ? "Medium"
                             : _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault() == 1 ? "Low" : "Not Rated"
            }).ToList();

            return loanApplications;
        }

        public LoanApplicationForReturnDTO GetLoanApplicationById(int id)
        {
            var loanApplication = _context.LOAN_APPLICATION.Where(x => x.LOANAPPLICATIONID == id).Select(x => new LoanApplicationForReturnDTO
            {
                customerName = _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.FIRSTNAME + " " + c.LASTNAME).FirstOrDefault() ?? "",
                productName = x.PRODUCTID == 1 ? "Term Loan" : x.PRODUCTID == 2 ? "Overdraft" : "Others",
                amount = x.AMOUNT,
                tenorInDays = x.TENOR,
                interestRate = x.INTERESTRATE,
                loanPurpose = x.LOANPURPOSE,
                currencyCode = x.CURRENCYID == 1 ? "NGN" : x.CURRENCYID == 2 ? "USD" : x.CURRENCYID == 3 ? "GBP" : x.CURRENCYID == 4 ? "EUR" : "Others" ,
                applicationDate = x.APPLICATIONDATE,
                approvalStatusId = x.APPROVALSTATUSID,
                statusName = _context.APPROVAL_STATUS.Where(a => a.APPROVALSTATUSID == x.APPROVALSTATUSID).Select(a => a.NAME).FirstOrDefault() ?? "Pending",
                loanApplicationId = x.LOANAPPLICATIONID,
                sectorName = _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 1 ? "Agriculture" :
                             _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 2 ? "Manufacturing" :
                             _context.CUSTOMER.Where(c => c.CUSTOMERID == x.CUSTOMERID).Select(c => c.SECTOR).First() == 3 ? "Services" : "Others",
                riskRating = _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault() == 3 ? "High"
                             : _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault() == 2 ? "Medium"
                             : _context.ESG_CHECKLIST_SUMMARY.Where(s => s.LOANAPPLICATIONID == x.LOANAPPLICATIONID).Select(s => s.RATINGID).FirstOrDefault() == 1 ? "Low" : "Not Rated"
            }).FirstOrDefault();

            return loanApplication!;
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