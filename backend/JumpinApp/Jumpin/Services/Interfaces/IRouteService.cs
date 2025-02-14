using Jumpin.Models;
using Jumpin.Models.DTO;

namespace Jumpin.Services.Interfaces
{
    public interface IRouteService
    {
        Task<CodeStatus> AddRoute(dtoUserRoute route);
        Task<CodeStatus> DeleteRoute(int routeId);
        Task<(CodeStatus, UserRoute)> UpdateRoute(dtoUserRoute route, int routeId);
        Task<(CodeStatus, List<UserRoute>)> GetRoutes();
        Task<(CodeStatus, List<UserRoute>)> GetUsersRoutes(string email);
        Task<(CodeStatus, UserRoute)> GetRouteDetailsById(int routeId);

    }
}
