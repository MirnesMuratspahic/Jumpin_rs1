using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jumpin.Services
{
    public class RatingService : IRatingService
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IConfiguration configuration;
        private readonly IErrorProviderService errorProviderService;
        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public RatingService(ApplicationDbContext dbContext, IConfiguration _configuration, IErrorProviderService _errorProviderService)
        {
            DbContext = dbContext;
            configuration = _configuration;
            errorProviderService = _errorProviderService;
        }

        public async Task<CodeStatus> AddRating(dtoRating dtoRating)
        {
            if (dtoRating == null) { return defaultError; }

            var userFromDatabaseWriting = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == dtoRating.UserWritingEmail);
            if(userFromDatabaseWriting == null) 
            {
                return await errorProviderService.GetCodeStatus("User not found!", true);
            }

            var userFromDatabaseRating = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == dtoRating.UsersRatingEmail);

            if (userFromDatabaseRating == null)
            {
                return await errorProviderService.GetCodeStatus("User not found!", true);
            }

            if(userFromDatabaseRating.Email == userFromDatabaseWriting.Email)
            {
                return await errorProviderService.GetCodeStatus("You can't leave a review for yourself!", true);
            }

            var newRating = new Rating()
            {
                Comment = dtoRating.Comment,
                Review = dtoRating.Review,
                DateTime = DateTime.Now
            };

            var newUserRating = new UserRating()
            {
                UsersRating = userFromDatabaseRating,
                UserWriting = userFromDatabaseWriting,
                Rating = newRating
            };

            await DbContext.Ratings.AddAsync(newRating);
            await DbContext.UserRating.AddAsync(newUserRating);
            await DbContext.SaveChangesAsync();

            return await errorProviderService.GetCodeStatus("Successfully created!", false);

        }

        public async Task<(CodeStatus, List<dtoUserRating>)> GetUsersRatings(string email)
        {
            if (string.IsNullOrEmpty(email))
                return (defaultError, null);

            var userFromDatabaseWriting = await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (userFromDatabaseWriting == null)
            {
                error = await errorProviderService.GetCodeStatus("User not found!", true);
                return (error, null);
            }

            var usersRatings = await DbContext.UserRating.Include(x => x.Rating).Include(x => x.UsersRating).Include(x => x.UserWriting)
                .Where(x => x.UsersRating.Email == email).Select(x => new dtoUserRating()
                {
                    Id = x.Rating.Id,
                    UserWritingEmail = x.UserWriting.Email,
                    RatingReview = x.Rating.Review,
                    Comment = x.Rating.Comment,
                    DateTime = x.Rating.DateTime
                }).ToListAsync();

            if(usersRatings.Count == 0 || usersRatings == null)
            {
                error = await errorProviderService.GetCodeStatus("No ratings!", true);
                return (error, null);
            }

            return (error, usersRatings);
        }

        public async Task<CodeStatus> DeleteRating(int ratingId)
        {
            if (ratingId < 0)
                return defaultError;

            var ratingFromDatabase = await DbContext.Ratings.FirstOrDefaultAsync(x=>x.Id == ratingId);
            var userRatingFromDatabase = await DbContext.UserRating.FirstOrDefaultAsync(x => x.Rating.Id == ratingId);

            if (ratingFromDatabase == null || userRatingFromDatabase == null)
                return await errorProviderService.GetCodeStatus("Rating not found!", true);

            DbContext.UserRating.Remove(userRatingFromDatabase);
            DbContext.Ratings.Remove(ratingFromDatabase);
            await DbContext.SaveChangesAsync();

            return await errorProviderService.GetCodeStatus("Successfully removed!", false);
        }


    }
}
