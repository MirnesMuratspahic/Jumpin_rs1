using Jumpin.Services.Interfaces;
using Microsoft.Identity.Client;
using Stripe;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;


namespace Jumpin.Services
{
    public class SmsService
    {
        public IConfiguration configuration { get; set; }
        private readonly IErrorProviderService errorProviderService;
        public SmsService(IConfiguration _configuration, IErrorProviderService _errorProviderService)
        {
            configuration = _configuration;
            errorProviderService = _errorProviderService;
        }

        public async Task SendVerificationCode(string phoneNumber, string code)
        {
            TwilioClient.Init(configuration["Twilio:AccountSid"], configuration["Twilio:AuthToken"]);
            string messageBody = $"Code for phone number verification: {code}";

            var message = await MessageResource.CreateAsync(
                body: messageBody,
                from: new Twilio.Types.PhoneNumber("+15705338812"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }
}
