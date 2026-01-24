using ESG.Api.Interface;
using ESG.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
        [EnableRateLimiting("GetAllLoanApplications")]
        public ActionResult<List<LoanApplicationForReturnDTO>> GetAllLoanApplication()
        {
            var loanApplications = _repo.GetAllLoanApplication();

            if (loanApplications != null)
            {
                return Ok(loanApplications);
            }

            return NotFound();
        }

        [HttpGet("{id:int}")]
        public ActionResult<LoanApplicationForReturnDTO> GetLoanApplicationById(int id)
        {
            var loanApplication = _repo.GetLoanApplicationById(id);

            if (loanApplication != null)
            {
                return Ok(loanApplication);
            }

            return NotFound(new { status = false, message = "Loan Application not found" });
        }

        [HttpPost("add")]
        [EnableRateLimiting("LoanCreate")]
        public ActionResult<CustomerForReturnDTO> CreateLoanApplication(LoanApplicationForCreationDTO loanApplicaion)
        {
            ArgumentNullException.ThrowIfNull(loanApplicaion);

            string refNumber = _repo.CreateLoanApplication(loanApplicaion);

            if (!string.IsNullOrEmpty(refNumber))
            {
                return Ok(new { status = true, result = refNumber, message = "Loan Application added successfully" });
            }

            return NotFound(new { status = false, message = "Loan Application not added. Kindly contact Admin" });
        }

        [HttpGet("submit-for-appraisal/{id:int}")]
        [EnableRateLimiting("AssessmentSubmit")]
        public async Task<IActionResult> SubmitLoanApplicationForAppraisalAsync(int id)
        {
            try
            {
                bool result = await _repo.SubmitLoanApplicationForAppraisalAsync(id);
                if (result)
                {
                    return Ok(new { success = true, message = "Loan application submitted for appraisal successfully." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to submit loan application for appraisal." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}