using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FlatController : ControllerBase
    {
        private readonly IFlatService flatService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();
        public FlatController(IFlatService _flatService, IErrorProviderService _errorProviderService)
        {
            flatService = _flatService;
            errorProviderService = _errorProviderService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("AddFlat")]
        public async Task<IActionResult> AddFlat(dtoUserFlat dtoFlat)
        {
            try
            {
                var error = await flatService.AddFlat(dtoFlat);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(new { error.Name });
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("GetFlats")]
        public async Task<IActionResult> GetFlats()
        {
            try
            {
                var (error, flats) = await flatService.GetFlats();
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(flats);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("DeleteFlat/{flatId}")]
        public async Task<IActionResult> DeleteFlat([FromRoute] int flatId)
        {
            try
            {
                var error = await flatService.DeleteFlat(flatId);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(new { error.Name });
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("UpdateFlat{flatId}")]
        public async Task<IActionResult> UpdateFlat(dtoUserFlat dtoFlat, int flatId)
        {
            try
            {
                var (error, car) = await flatService.UpdateFlat(dtoFlat, flatId);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(car);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetUserFlats")]
        public async Task<IActionResult> GetUserFlats([FromBody] string email)
        {
            try
            {
                var (error, flats) = await flatService.GetUserFlats(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(flats);
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