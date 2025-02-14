using Jumpin.Models;
using Jumpin.Models.DTO;

namespace Jumpin.Services.Interfaces
{
    public interface IRatingService
    {
        Task<CodeStatus> AddRating(dtoRating dtoRating);
        Task<(CodeStatus, List<dtoUserRating>)> GetUsersRatings(string email);
        Task<CodeStatus> DeleteRating(int ratingId);
    }
}
