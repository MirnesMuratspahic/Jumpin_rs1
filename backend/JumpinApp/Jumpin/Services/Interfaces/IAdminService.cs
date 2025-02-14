using Jumpin.Models;

namespace Jumpin.Services.Interfaces
{
    public interface IAdminService
    {
        Task<(CodeStatus, List<object>)> GetAppInformations();
    }
}
