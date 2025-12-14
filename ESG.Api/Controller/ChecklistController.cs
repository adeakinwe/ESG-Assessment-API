using ESG.Api.DTos;
using ESG.Api.Interface;
using ESG.API.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace ESG.API.Controller
{
    [Route("api/checklist")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistRepo _repo;

        public ChecklistController(IChecklistRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("checklist-item")]
        public async Task<IActionResult> GetChecklistAsync()
        {
            var data = await _repo.GetChecklistAsync();
            return Ok(data);
        }

        [HttpPost("submit-esg-assessment")]
        public async Task<IActionResult> SubmitChecklistAssessmentAsync([FromBody] EsgChecklistSubmissionDto dto)
        {
            try
            {
                if (dto.Items == null || !dto.Items.Any())
                    return BadRequest("No checklist responses provided");

                await _repo.SubmitChecklistAssessmentAsync(dto);
                return Ok(new { success = true, message = "ESG assessment submitted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("loan-application/{loanApplicationId:int}")]
        public async Task<IActionResult> GetChecklistAssessmentByLoanIdAsync(int loanApplicationId)
        {
            var assessment = await _repo.GetChecklistAssessmentByLoanIdAsync(loanApplicationId);

            if (assessment != null)
            {
                return Ok(new { status = true, data = assessment });
            }

            return NotFound(new { status = false, message = "No record found" });
        }

        [HttpGet("checklist-summary/{loanApplicationId:int}")]
        public async Task<IActionResult> GetChecklistAssessmentSummaryByLoanIdAsync(int loanApplicationId)
        {
            var existingSummary = await _repo.GetChecklistAssessmentSummaryByLoanIdAsync(loanApplicationId);

            if (existingSummary != null)
            {
                return Ok(new { status = true, data = existingSummary });
            }

            return NotFound(new { status = false, message = "No record found" });
        }
    }
}