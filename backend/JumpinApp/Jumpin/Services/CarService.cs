using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Jumpin.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration configuration;
        private readonly IErrorProviderService errorProviderService;
        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public CarService(ApplicationDbContext dbContext, IConfiguration _configuration, IErrorProviderService _errorProviderService)
        {
            DbContext = dbContext;
            configuration = _configuration;
            errorProviderService = _errorProviderService;
        }

        public async Task<CodeStatus> AddCar(dtoUserCar car)
        {
            if (car == null)
                return defaultError;
            var email = car.Email;

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("User not found!", true);
            }

            var newCar = new Car()
            {
                Name = car.Name,
                Description = car.Description,
                DateAndTime = car.DateAndTime,
                Price = car.Price,
                Type = car.Type,
                Status = "Active"
            };

            await DbContext.Cars.AddAsync(newCar);
            await DbContext.SaveChangesAsync();

            var newUserCar = new UserCar()
            {
                User = userFromDatabase,
                Car = newCar
            };

            await DbContext.UserCars.AddAsync(newUserCar);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully created!", false);
        }

        public async Task<(CodeStatus, List<UserCar>)> GetCars()
        {
            var cars = await DbContext.UserCars.Include(x => x.User).Include(x => x.Car).Where(x => x.Car.Status == "Active").ToListAsync();

            if (cars == null || cars.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No cars created!", true);
            }

            return (error, cars);
        }

        public async Task<CodeStatus> DeleteCar(int carId)
        {
            if (carId < 0)
            {
                return error = await errorProviderService.GetCodeStatus("Invalid ID", true);
            }

            var carFromDataBase = await DbContext.UserCars.FirstOrDefaultAsync(x => x.Car.Id == carId);

            var onlyCar = await DbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);

            if (carFromDataBase == null || onlyCar == null)
            {
                return error = await errorProviderService.GetCodeStatus("Car not found!", true);
            }

            DbContext.UserCars.Remove(carFromDataBase);
            DbContext.Cars.Remove(onlyCar);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully removed!", false);
        }

        public async Task<(CodeStatus, UserCar)> UpdateCar(dtoUserCar car, int carId)
        {
            if (car == null)
            {
                return (defaultError, null);
            }

            if (carId < 0)
            {
                error = await errorProviderService.GetCodeStatus("Invalid ID", true);
                return (error, null);
            }

            var carFromDatabase = await DbContext.UserCars.Include(x => x.User).Include(x => x.Car).FirstOrDefaultAsync(x => x.Car.Id == carId);

            var onlyCar = await DbContext.Cars.FirstOrDefaultAsync(x => x.Id == carId);

            if (carFromDatabase == null || onlyCar == null)
            {
                error = await errorProviderService.GetCodeStatus("Car not found!", true);
                return (error, null);
            }

            onlyCar.Name = car.Name != null ? car.Name : onlyCar.Name;
            onlyCar.DateAndTime = car.DateAndTime != null ? car.DateAndTime : onlyCar.DateAndTime;
            onlyCar.Price = car.Price >= 0 ? car.Price : onlyCar.Price;
            onlyCar.Description = car.Description != null ? car.Description : onlyCar.Description;
            onlyCar.Type = car.Type != null ? car.Type : onlyCar.Type;

            DbContext.Cars.Update(onlyCar);
            DbContext.UserCars.Update(carFromDatabase);
            await DbContext.SaveChangesAsync();

            return (error, carFromDatabase);
        }

        public async Task<(CodeStatus, List<UserCar>)> GetUsersCars(string email)
        {
            var cars = await DbContext.UserCars.Where(x => x.User.Email == email).Include(x => x.User).Include(x => x.Car).ToListAsync();

            if (cars == null || cars.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No cars created!", true);
                return (error, null);
            }

            return (error, cars);
        }
    }
}
