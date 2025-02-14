using Jumpin.Models;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.V2;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IErrorProviderService errorProviderService;
        private readonly StripeService stripeService;
        private CodeStatus error = new CodeStatus();

        public StripeController(IErrorProviderService _errorProviderService, StripeService _stripeService )
        {
            errorProviderService = _errorProviderService;
            stripeService = _stripeService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("CheckoutSesion/{amount}")]
        public async Task<IActionResult> CreateCheckoutSession([FromRoute] float amount)
        {
            try
            {
                var sessionId = await stripeService.CreateCheckoutSessionAsync(amount);
                return Ok(sessionId);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
