using Jumpin.Models;

namespace Jumpin.Services.Interfaces
{
    public interface IArtificialIntelligenceService
    {
        Task<(CodeStatus, string)> OpenAIRequest(string question);
    }
}
