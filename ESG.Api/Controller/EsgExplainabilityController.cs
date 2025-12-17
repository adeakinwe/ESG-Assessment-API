using ESG.Api.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Api.Controller
{
    [ApiController]
    [Route("api/esg/explainability")]
    public class EsgExplainabilityController : ControllerBase
    {
        private readonly IEsgExplainabilityService _service;

        public EsgExplainabilityController(IEsgExplainabilityService service)
        {
            _service = service;
        }

        [HttpGet("{loanApplicationId}")]
        public async Task<IActionResult> GenerateExplainabilityAsync(int loanApplicationId)
        {
            var result = await _service.GenerateExplainabilityAsync(loanApplicationId);
            return Ok(result);
        }
    }
}