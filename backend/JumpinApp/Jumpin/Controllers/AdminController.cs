using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private IAdminService adminService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();

        public AdminController(IAdminService _adminService, IErrorProviderService _errorProviderService)
        {
            adminService = _adminService;
            errorProviderService = _errorProviderService;
        }

        [HttpGet("GetAppInformations")]
        public async Task<IActionResult> GetAppInformations()
        {
            try
            {
                var (error, informations) = await adminService.GetAppInformations();
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(informations);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }
    }
}
