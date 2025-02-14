using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Jumpin.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService requestService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();
        public RequestController(IRequestService _requestService, IErrorProviderService _errorProviderService)
        {
            requestService = _requestService;
            errorProviderService = _errorProviderService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("SendRequest")]
        public async Task<IActionResult> SendRequest(dtoRequest dtoRequest)
        {
            try
            {
                var error = await requestService.SendRequest(dtoRequest);
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
        [HttpPost("AcceptOrDeclineRequest")]
        public async Task<IActionResult> AcceptOrDeclineRequest(dtoRequestAcceptOrDecline dtoRequest)
        {
            try
            {
                var error = await requestService.AcceptOrDeclineRequest(dtoRequest);
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
        [HttpPost("GetRouteRequestsRecived")]
        public async Task<IActionResult> GetRouteRequestsRecived([FromBody] string email)
        {
            try
            {
                var (error, requests) = await requestService.GetRouteRequestsRecived(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(requests);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }

        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetRouteSentRequests")]
        public async Task<IActionResult> GetRouteSentRequests([FromBody] string email)
        {
            try
            {
                var (error, requests) = await requestService.GetRouteSentRequests(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(requests);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetCarSentRequests")]
        public async Task<IActionResult> GetCarSentRequests([FromBody] string email)
        {
            try
            {
                var (error, requests) = await requestService.GetCarSentRequests(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(requests);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetCarRequestsRecived")]
        public async Task<IActionResult> GetCarRequestsRecived([FromBody] string email)
        {
            try
            {
                var (error, requests) = await requestService.GetCarRequestsRecived(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(requests);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }

        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetFlatSentRequests")]
        public async Task<IActionResult> GetFlatSentRequests([FromBody] string email)
        {
            try
            {
                var (error, requests) = await requestService.GetFlatSentRequests(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(requests);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetFlatRequestsRecived")]
        public async Task<IActionResult> GetFlatRequestsRecived([FromBody] string email)
        {
            try
            {
                var (error, requests) = await requestService.GetFlatRequestsRecived(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(requests);
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
