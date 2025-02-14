using AlarmSystem.Models.DTO;
using Jumpin.Models;
using Jumpin.Models.DTO;

namespace Jumpin.Services.Interfaces
{
    public interface IUserService
    {

        Task<List<User>> GetUsers();
        Task<CodeStatus> UserRegistration(dtoUserRegistration user);
        Task<(CodeStatus, string)> UserLogin(dtoUserLogin user);
        Task<CodeStatus> ResendCode(string email, string type);
        Task<CodeStatus> AcceptUserCode(dtoUserCode userCode);
        Task<(CodeStatus, User)> GetUserInformations(string email);
        Task<CodeStatus> UpgradeUsersAccount(string email);
        Task<CodeStatus> UpdateUserInformations(dtoUserInformations dtoUserInformation);
        Task<CodeStatus> SendEmailForPasswordReset(string email);
        Task<CodeStatus> ResetPassword(string email, string password);
        Task<CodeStatus> SendSmsForPhoneVerification(string email);
        Task<CodeStatus> ChangeUserStatus(int status, string email);

    }
}
