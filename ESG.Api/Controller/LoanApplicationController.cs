using ESG.Api.Interface;
using ESG.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Api.Controller
{
    [Route("api/loan-application")]
    [ApiController]
    public class LoanApplicationController : ControllerBase
    {
        private readonly ILoanApplicationRepo _repo;

        public LoanApplicationController(ILoanApplicationRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("all")]
        public ActionResult<List<LoanApplicationForReturnDTO>> GetAllLoanApplication()
        {
            var loanApplications = _repo.GetAllLoanApplication();

            if (loanApplications != null)
            {
               return Ok(loanApplications) ; 
            }

            return NotFound();
        }

        [HttpGet("{id:int}")]
        public ActionResult<LoanApplicationForReturnDTO> GetLoanApplicationById(int id)
        {
            var loanApplication = _repo.GetLoanApplicationById(id);

            if (loanApplication != null)
            {
               return Ok(loanApplication) ; 
            }

            return NotFound(new { status = false, message="Loan Application not found" });
        }

        [HttpPost("add")]
        public ActionResult<CustomerForReturnDTO> CreateLoanApplication(LoanApplicationForCreationDTO loanApplicaion)
        {
            ArgumentNullException.ThrowIfNull(loanApplicaion);

            bool status = _repo.CreateLoanApplication(loanApplicaion);

            if (status)
            {
               return Ok() ; 
            }

            return NotFound(new { status = false, message="Loan Application not added. Kindly contact Admin" });
        }
    }
}