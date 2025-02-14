using AlarmSystem.Services.Interfaces;
using Jumpin.Models;
using Jumpin.Services;
using Jumpin.Services.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace AlarmSystem.Services
{
    public class EmailService : IEmailService
    {
        public IConfiguration configuration { get; set; }
        private readonly IErrorProviderService errorProviderService;
        private readonly PdfService pdfService;
        public EmailService(IConfiguration _configuration, IErrorProviderService _errorProviderService, PdfService _pdfService)
        {
            configuration = _configuration;
            errorProviderService = _errorProviderService;
            pdfService = _pdfService;
        }

        public async Task SendEmailWithCode(string code, string email, string type)
        {
            MailMessage mail = new MailMessage();
            string subject = "";
            string message = "";

            if (type == "email")
            {
                message = "Dear User, \n" + "\n" +
                          $"Your code for email address verification is as follows: {code} \n" + "\n" +
                          $"Verification link: http://localhost:4200/code?email={email} \n" +
                          "Best regards, \n" +
                          "AlarmSystem";

                subject = "Email Address Verification";
            }
            else if (type == "2fa")
            {
                message = "Dear User, \n" + "\n" +
                          $"Your verification code is as follows: {code} \n" + "\n" +
                          $"Verification link: http://localhost:4200/2fa?email={email} \n" +
                          "Best regards, \n" +
                          "AlarmSystem";

                subject = "2FA Verification";
            }
            else if (type == "password")
            {
                message = "Dear User, \n" + "\n" +
                          $"Reset your password using the following link: \n" + "\n" +
                          $"http://localhost:4200/password-reset-page?email={email} \n" +
                          "Best regards, \n" +
                          "AlarmSystem";

                subject = "Password Reset";
            }


            mail.From = new MailAddress(configuration["EmailAccountInformations:Email"]);
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;



            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(configuration["EmailAccountInformations:Email"], configuration["EmailAccountInformations:Password"]);
            smtpServer.EnableSsl = true;

            try
            {
                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {
            }


        }


        public async Task SendPdfEmail(Request requestData, int answer = 3)
        {
            byte[] pdfContent;
            MailMessage mail = new MailMessage();
            string subject = "Reservation request";
            string message = "Please find the attached PDF document regarding your request.";

            string outputName = $"{requestData.PassengerEmail}_Request_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            
            if(answer == 0)
            {
                pdfContent = pdfService.AnswerRequestPdf(answer, requestData);
            }
            else if (answer == 1)
            {
                pdfContent = pdfService.AnswerRequestPdf(answer, requestData);
            }
            else
            {
                pdfContent = pdfService.CreateRequestPdf(requestData);
            }

            var pdfAttachment = new Attachment(new MemoryStream(pdfContent), "request.pdf", "application/pdf");
            mail.Attachments.Add(pdfAttachment);

            mail.From = new MailAddress(configuration["EmailAccountInformations:Email"]);
            mail.To.Add(requestData.PassengerEmail);
            mail.Subject = subject;
            mail.Body = message;

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(configuration["EmailAccountInformations:Email"], configuration["EmailAccountInformations:Password"]);
            smtpServer.EnableSsl = true;

            try
            {
                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {
            }
        }


    }

}
