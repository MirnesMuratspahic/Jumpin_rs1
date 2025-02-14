using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private  readonly IRatingService ratingService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();

        public RatingController(IRatingService _ratingService, IErrorProviderService _errorProviderService)
        {
            ratingService = _ratingService;
            errorProviderService = _errorProviderService;
        }

        //[Authorize(Roles = "User, Admin")]
        [HttpPost("AddRating")]
        public async Task<IActionResult> AddRating(dtoRating dtoRating)
        {
            try
            {
                var error = await ratingService.AddRating(dtoRating);
                if (error.ErrorStatus == true)
                    return BadRequest(error.Name);
                return Ok(error.Name);

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetUsersRatings")]
        public async Task<IActionResult> GetUsersRatings([FromBody] string email)
        {
            try
            {
                var (error, userRatings) = await ratingService.GetUsersRatings(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { name = error.Name });
                return Ok(userRatings);

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("DeleteRating/{ratingId}")]
        public async Task<IActionResult> DeleteRating([FromRoute] int ratingId)
        {
            try
            {
                var error = await ratingService.DeleteRating(ratingId);
                if (error.ErrorStatus == true)
                    return BadRequest(error.Name);
                return Ok(error.Name);

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }
        }
    }
}
