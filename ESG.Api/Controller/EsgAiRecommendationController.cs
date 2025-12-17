using ESG.Api.Contracts;
using ESG.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Api.Controller
{
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

        [HttpPost("final-recommendation")]
        public async Task<IActionResult> Final([FromBody] FinalAssessmentRequest request)
        {
            var result = await _service.FinalRecommendationAsync(request);
            return Ok(result);
        }
    }
}