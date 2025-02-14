using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jumpin.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration configuration;
        private readonly IErrorProviderService errorProviderService;
        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public AdminService(ApplicationDbContext dbContext, IConfiguration _configuration, IErrorProviderService _errorProviderService)
        {
            DbContext = dbContext;
            configuration = _configuration;
            errorProviderService = _errorProviderService;
        }


        public async Task<(CodeStatus, List<object>)> GetAppInformations()
        {
            try
            {
                var usersCount = await DbContext.Users.CountAsync();
                var routesCount = await DbContext.Routes.CountAsync();
                var carsCount = await DbContext.Cars.CountAsync();
                var flatsCount = await DbContext.Flats.CountAsync();

                var result = new List<object>()
                {
                    new { Name = "Users", Count =  usersCount },
                    new { Name = "Routes", Count =  routesCount },
                    new { Name = "Cars", Count =  carsCount },
                    new { Name = "Flats", Count =  flatsCount },
                };

                return (error,  result);
            }
            catch(Exception ex) {
                return (defaultError, new List<object>());
            }
        }
    }
}
