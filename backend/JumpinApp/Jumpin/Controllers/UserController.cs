using Jumpin.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Jumpin.Models;
using AlarmSystem.Models.DTO;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();
        public UserController(IUserService _userService, IErrorProviderService _errorProviderService)
        {
            userService = _userService;
            errorProviderService = _errorProviderService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await userService.GetUsers();
                return Ok(users);

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> UserRegistration(dtoUserRegistration user)
        {
            try
            {
                var error = await userService.UserRegistration(user);
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

        [HttpPost("AcceptUserCode")]
        public async Task<IActionResult> AcceptUserCode(dtoUserCode dtoUserCode)
        {
            try
            {
                var error = await userService.AcceptUserCode(dtoUserCode);
                if (error.ErrorStatus == true)
                    return BadRequest(new { name = error.Name });
                return Ok(new { name = error.Name });
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }
            
        }

        [HttpPost("ResendCode/{type}")]
        public async Task<IActionResult> ResendCode([FromBody] string email, [FromRoute] string type)
        {
            try
            {
                var error = await userService.ResendCode(email, type);
                if (error.ErrorStatus == true)
                    return BadRequest(error.Name);
                return Ok(error.Name );
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(dtoUserLogin user)
        {
            try
            {
                var (error, token) = await userService.UserLogin(user);
                if (error.ErrorStatus == true)
                    return BadRequest(error.Name);
                return Ok(token);

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name);
            }
        }


        [Authorize(Roles = "User, Admin")]
        [HttpPost("GetUserInformations")]
        public async Task<IActionResult> GetUserInformations([FromBody] string email)
        {
            try
            {
                var (error, user) = await userService.GetUserInformations(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(user);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ChangeUserStatus/{status}")]
        public async Task<IActionResult> ChangeUserStatus([FromRoute] int status, [FromBody] string email)
        {
            try
            {
                var error = await userService.ChangeUserStatus(status, email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(error.Name);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }


        [Authorize(Roles = "User")]
        [HttpPost("UpgradeUsersAccount")]
        public async Task<IActionResult> UpgradeUsersAccount([FromBody] string email)
        {
            try
            {
                var error = await userService.UpgradeUsersAccount(email);
                if (error.ErrorStatus == true)
                    return BadRequest(error.Name);
                return Ok(error.Name);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, error.Name );
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("UpdateUserInformations")]
        public async Task<IActionResult> UpdateUserInformations(dtoUserInformations userInformations)
        {
            try
            {
                var error = await userService.UpdateUserInformations(userInformations);
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
        [HttpPost("SendEmailForPasswordReset")]
        public async Task<IActionResult> SendEmailForPasswordReset([FromBody] string email)
        {
            try
            {
                var error = await userService.SendEmailForPasswordReset(email);
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

        //[Authorize(Roles = "User, Admin")]
        [HttpPost("SendSmsForPhoneVerification")]
        public async Task<IActionResult> SendSmsForPhoneVerification([FromBody] string email)
        {
            try
            {
                var error = await userService.SendSmsForPhoneVerification(email);
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
        [HttpPost("ResetPassword/{email}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string email, [FromBody] string password)
        {
            try
            {
                var error = await userService.ResetPassword(email,password);
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
