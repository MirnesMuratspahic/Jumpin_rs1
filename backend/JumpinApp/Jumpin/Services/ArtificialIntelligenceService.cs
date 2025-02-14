using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Services.Interfaces;
using OpenAI_API;
using OpenAI_API.Chat;


namespace Jumpin.Services
{
    public class ArtificialIntelligenceService : IArtificialIntelligenceService
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IErrorProviderService errorProviderService;
        public CodeStatus error = new CodeStatus() { ErrorStatus = false };
        public CodeStatus defaultError = new CodeStatus() { ErrorStatus = true, Name = "The property must not be null" };

        public ArtificialIntelligenceService(ApplicationDbContext _context, IConfiguration _configuration)
        {
            this.context = _context;
            this.configuration = _configuration;
        }

        public async Task<(CodeStatus, string)> OpenAIRequest(string question)
        {
            if (question == null || question.Length == 0)
            {
                return (defaultError, null);
            }

            var authentication = new APIAuthentication(configuration["OpenAI:ApiKey"]);
            var api = new OpenAIAPI(authentication);
            var conversation = api.Chat.CreateConversation();

            var input = question;

            conversation.AppendUserInput(question);

            var response = await conversation.GetResponseFromChatbot();

            return (error, response);
        }
    }

}
