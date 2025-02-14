using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService carService;
        private readonly IErrorProviderService errorProviderService;
        private CodeStatus error = new CodeStatus();
        public CarController(ICarService _carService, IErrorProviderService _errorProviderService)
        {
            carService = _carService;
            errorProviderService = _errorProviderService;
        }


        [Authorize(Roles = "User, Admin")]
        [HttpPost("AddCar")]
        public async Task<IActionResult> AddCar(dtoUserCar dtoCar)
        {
            try
            {
                var error = await carService.AddCar(dtoCar);
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
        [HttpGet("GetCars")]
        public async Task<IActionResult> GetCars()
        {
            try
            {
                var (error, cars) = await carService.GetCars();
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(cars);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                error = await errorProviderService.GetCodeStatus(ex.Message, true);
                return StatusCode(500, new { error.Name });
            }
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("DeleteCar/{carId}")]
        public async Task <IActionResult> DeleteCar([FromRoute] int carId)
        {
            try
            {
                var error = await carService.DeleteCar(carId);
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
        [HttpPut("UpdateCar{carId}")]
        public async Task<IActionResult> UpdateRoute([FromBody] dtoUserCar dtoCar, [FromRoute] int carId)
        {
            try
            {
                var (error, car) = await carService.UpdateCar(dtoCar, carId);
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
        [HttpPost("GetUsersCars")]
        public async Task<IActionResult> GetUserCars([FromBody] string email)
        {
            try
            {
                var (error, cars) = await carService.GetUsersCars(email);
                if (error.ErrorStatus == true)
                    return BadRequest(new { error.Name });
                return Ok(cars);
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
