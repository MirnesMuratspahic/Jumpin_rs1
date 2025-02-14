using Jumpin.Models;

namespace AlarmSystem.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailWithCode(string code, string email, string type);
        Task SendPdfEmail(Request requestData, int answer = 3);
    }
}
