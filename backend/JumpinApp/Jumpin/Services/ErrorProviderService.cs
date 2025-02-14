using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace Jumpin.Services
{
    public class ErrorProviderService : IErrorProviderService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly ITokenHelperService tokenHelperService;
        public ErrorProviderService(ApplicationDbContext _dbContext, ITokenHelperService _tokenHelperService) 
        {
            DbContext = _dbContext;
            tokenHelperService = _tokenHelperService;
        }

        public async Task<CodeStatus> GetCodeStatus(string name, bool status)
        {
            User userFromDatabase = new User();
            var email = await tokenHelperService.GetEmailFromToken();
            if(!string.IsNullOrEmpty(email)) 
                userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x=>x.Email == email);
            if(userFromDatabase == null) 
                return new CodeStatus(name, status, null);
            var error = new CodeStatus(name, status, userFromDatabase);
            if(status)
            {
                await DbContext.Errors.AddAsync(error);
                await DbContext.SaveChangesAsync();
            }
            return error;
        }
    }
}
