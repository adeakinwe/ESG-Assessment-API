using ESG.Api.Contracts;
using ESG.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ESG.Api.Controller
{
    [EnableRateLimiting(RateLimitPolicies.Sensitive)]
    [ApiController]
    [Route("api/esg-ai-recommendation")]
    public class EsgAiRecommendationController : ControllerBase
    {
        private readonly IEsgAiRecommendationService _service;

        public EsgAiRecommendationController(IEsgAiRecommendationService service)
        {
            _service = service;
        }

        [HttpPost("pre-screen")]
        public async Task<IActionResult> PreScreen([FromBody] PreScreenRequest request)
        {
            var result = await _service.PreScreenAsync(request);
            return Ok(result);
        }

        [HttpGet("final/{loanApplicationId:int}")]
        [EnableRateLimiting("AiRecommendation")]
        public async Task<IActionResult> FinalRecommendation(int loanApplicationId)
        {
            var result = await _service.GenerateFinalRecommendationAsync(loanApplicationId);
            return Ok(result);
        }

        [HttpPost("final-recommendation-alt")]
        public async Task<IActionResult> Final([FromBody] FinalAssessmentRequest request)
        {
            var result = await _service.FinalRecommendationAsync(request);
            return Ok(result);
        }
    }
}