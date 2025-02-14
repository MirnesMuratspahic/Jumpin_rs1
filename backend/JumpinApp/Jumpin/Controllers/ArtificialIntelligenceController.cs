using Jumpin.Models;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AIController : Controller
    {
        private readonly IArtificialIntelligenceService aiService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();

        public AIController(IArtificialIntelligenceService _aiService, IErrorProviderService _errorProviderService)
        {
            aiService = _aiService;
            errorProviderService = _errorProviderService;
        }

        [HttpPost("AIQuery")]
        public async Task<IActionResult> OpenAIRequest([FromBody] string request)
        {
            var (error, message) = await aiService.OpenAIRequest(request);
            if (error.ErrorStatus == true)
                return BadRequest(error.Name);
            return Ok(message);
        }
    }
}
