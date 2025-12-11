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
    }
}