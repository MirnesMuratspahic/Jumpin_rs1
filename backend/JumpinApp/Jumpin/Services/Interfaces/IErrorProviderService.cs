using Jumpin.Models;

namespace Jumpin.Services.Interfaces
{
    public interface IErrorProviderService
    {
        Task<CodeStatus> GetCodeStatus(string name, bool status);
    }
}
