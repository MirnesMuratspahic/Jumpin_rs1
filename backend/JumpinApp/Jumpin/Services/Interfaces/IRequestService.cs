using Jumpin.Models;
using Jumpin.Models.DTO;

namespace Jumpin.Services.Interfaces
{
    public interface IRequestService
    {
        Task<CodeStatus> SendRequest(dtoRequest dtoRequest);
        Task<CodeStatus> AcceptOrDeclineRequest(dtoRequestAcceptOrDecline dtoRequest);
        Task<(CodeStatus, List<RouteRequest>)> GetRouteRequestsRecived(string email);
        Task<(CodeStatus, List<RouteRequest>)> GetRouteSentRequests(string email);
        Task<(CodeStatus, List<CarRequest>)> GetCarRequestsRecived(string email);
        Task<(CodeStatus, List<CarRequest>)> GetCarSentRequests(string email);
        Task<(CodeStatus, List<FlatRequest>)> GetFlatRequestsRecived(string email);
        Task<(CodeStatus, List<FlatRequest>)> GetFlatSentRequests(string email);
    }
}
