namespace Jumpin.Services.Interfaces
{
    public interface ITokenHelperService
    {
        Task<string> GetEmailFromToken();
    }
}
