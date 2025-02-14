using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Jumpin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jumpin.Models;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService routeService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();

        public RouteController(IRouteService _routeService, IErrorProviderService _errorProviderService)
        {
            routeService = _routeService;
            errorProviderService = _errorProviderService;
        }


        [Authorize(Roles = "User, Admin")]
        [HttpPost("AddRoute")]
        public async Task<IActionResult> AddRoute(dtoUserRoute dtoRoute)
        {
            try
            {
                var error = await routeService.AddRoute(dtoRoute);
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
        [HttpDelete("DeleteRoute/{routeId}")]
        public async Task<IActionResult> DeleteRoute([FromRoute] int routeId)
        {
            try
            {
                var error = await routeService.DeleteRoute(routeId);
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
        [HttpPut("UpdateRoute/{routeId}")]
        public async Task<IActionResult> UpdateRoute([FromBody] dtoUserRoute dtoRoute, [FromRoute] int routeId)
        {
            try
            {
                var (error, route) = await routeService.UpdateRoute(dtoRoute, routeId);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(route);

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("GetRoutes")]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                var (error, routes) = await routeService.GetRoutes();
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(routes);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetUsersRoutes")]
        public async Task<IActionResult> GetUsersRoutes([FromBody] string email)
        {
            try
            {
                var (error, routes) = await routeService.GetUsersRoutes(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(routes);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("GetRouteDetails/{routeId}")]
        public async Task<IActionResult> GetRouteDetails([FromRoute] int routeId)
        {
            try
            {
                var (error, routeDetails) = await routeService.GetRouteDetailsById(routeId);
                if (error.ErrorStatus)
                    return BadRequest(new { error.Name });
                return Ok(routeDetails);
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
