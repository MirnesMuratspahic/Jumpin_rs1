using Jumpin.Models;
using Jumpin.Models.DTO;

namespace Jumpin.Services.Interfaces
{
    public interface ICarService
    {
        Task<CodeStatus> AddCar(dtoUserCar car);
        Task<(CodeStatus, List<UserCar>)> GetCars();
        Task<CodeStatus> DeleteCar(int routeId);
        Task <(CodeStatus, UserCar)> UpdateCar(dtoUserCar car, int carId);
        Task<(CodeStatus, List<UserCar>)> GetUsersCars(string email);
    }
}
