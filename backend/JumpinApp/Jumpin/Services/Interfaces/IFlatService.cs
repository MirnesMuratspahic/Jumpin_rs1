using Jumpin.Models;
using Jumpin.Models.DTO;

namespace Jumpin.Services.Interfaces
{
    public interface IFlatService
    {
        Task<CodeStatus> AddFlat(dtoUserFlat flat);
        Task<(CodeStatus, List<UserFlat>)> GetFlats();
        Task<CodeStatus> DeleteFlat(int flatId);
        Task<(CodeStatus, UserFlat)> UpdateFlat(dtoUserFlat flat, int flatId);
        Task<(CodeStatus, List<UserFlat>)> GetUserFlats(string email);
    }
}
