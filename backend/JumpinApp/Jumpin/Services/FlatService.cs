using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jumpin.Services
{
    public class FlatService : IFlatService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration configuration;
        private readonly IErrorProviderService errorProviderService;
        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public FlatService(ApplicationDbContext dbContext, IConfiguration _configuration, IErrorProviderService _errorProviderService)
        {
            DbContext = dbContext;
            configuration = _configuration;
            errorProviderService = _errorProviderService;
        }

        public async Task<CodeStatus> AddFlat(dtoUserFlat flat)
        {
            if (flat == null)
                return defaultError;
            var email = flat.Email;


            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (userFromDatabase == null)
            {
                return error = await errorProviderService.GetCodeStatus("User not found!", true);
            }

            var newFlat = new Flat()
            {
                Name = flat.Name,
                Description = flat.Description,
                DateAndTime = flat.DateAndTime,
                Price = flat.Price,
                Type = flat.Type,
                Status = "Active"
            };


            await DbContext.Flats.AddAsync(newFlat);
            await DbContext.SaveChangesAsync();

            var newUserFlat = new UserFlat()
            {
                User = userFromDatabase,
                Flat = newFlat
            };


            await DbContext.UserFlats.AddAsync(newUserFlat);
            await DbContext.SaveChangesAsync();


            return error = await errorProviderService.GetCodeStatus("Successfully created!", false);
        }

        public async Task<(CodeStatus, List<UserFlat>)> GetFlats()
        {
            var flats = await DbContext.UserFlats.Include(x => x.User).Include(x => x.Flat).Where(x => x.Flat.Status == "Active").ToListAsync();

            if (flats == null || flats.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No flats created!", true);
                return (error, null);
            }

            return (error, flats);
        }

        public async Task<CodeStatus> DeleteFlat(int flatId)
        {
            if (flatId < 0)
            {
                return error = await errorProviderService.GetCodeStatus("Invalid ID", true);
            }

            var flatFromDatabase = await DbContext.UserFlats.FirstOrDefaultAsync(x => x.Flat.Id == flatId);

            var onlyFlat = await DbContext.Flats.FirstOrDefaultAsync(x => x.Id == flatId);

            if(flatFromDatabase == null || onlyFlat == null)
            {
                return error = await errorProviderService.GetCodeStatus("Flat not found!", true);
            }

            DbContext.UserFlats.Remove(flatFromDatabase);
            DbContext.Flats.Remove(onlyFlat);
            await DbContext.SaveChangesAsync();

            return error = await errorProviderService.GetCodeStatus("Successfully removed!", false);
        }

        public async Task<(CodeStatus, UserFlat)> UpdateFlat(dtoUserFlat flat, int flatId)
        {
            if(flat == null)
            {
                return (defaultError, null);
            }

            if (flatId < 0)
            {
                error = await errorProviderService.GetCodeStatus("Invalid ID", true);
                return (error, null);
            }

            var flatFromDatabase = await DbContext.UserFlats.Include(x => x.User).Include(x => x.Flat).FirstOrDefaultAsync(x => x.Flat.Id == flatId);

            var onlyFlat = await DbContext.Flats.FirstOrDefaultAsync(x => x.Id == flatId);

            if (flatFromDatabase == null || onlyFlat == null)
            {
                error = await errorProviderService.GetCodeStatus("Flat not found!", true);
                return (error, null);
            }

            onlyFlat.Name = flat.Name != null ? flat.Name : onlyFlat.Name;
            onlyFlat.DateAndTime = flat.DateAndTime != null ? flat.DateAndTime : onlyFlat.DateAndTime;
            onlyFlat.Price = flat.Price != null ? flat.Price : onlyFlat.Price;
            onlyFlat.Description = flat.Description != null ? flat.Description : onlyFlat.Description;
            onlyFlat.Type = flat.Type != null ? flat.Type : onlyFlat.Type;

            DbContext.Flats.Update(onlyFlat);
            DbContext.UserFlats.Update(flatFromDatabase);
            await DbContext.SaveChangesAsync();

            return (error, flatFromDatabase);
        }

        public async Task<(CodeStatus, List<UserFlat>)> GetUserFlats(string email)
        {
            var flats = await DbContext.UserFlats.Where(x => x.User.Email == email).Include(x => x.User).Include(x => x.Flat).ToListAsync();

            if(flats == null || flats.Count == 0)
            {
                error = await errorProviderService.GetCodeStatus("No flats created!", true);
                return (error, null);
            }
            return (error, flats);
        }
    }
}
