using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Infrastructure.Services.Settings;
using Mailtrap;
using Mailtrap.Emails.Requests;
using Microsoft.Extensions.Options;
using MailAttachment = Mailtrap.Emails.Models.Attachment;


namespace AsyncPlate.Infrastructure.Services
{
    public class MailTrapEmailService : IEmailService
    {
        private readonly IMailtrapClient _mailtrapClient; //will be injected using the line of configuration in the program.cs
        private readonly MailtarpMappingClass _settings;

        public MailTrapEmailService(IMailtrapClient mailtrapClient, IOptions<MailtarpMappingClass> settings)
        {
            _mailtrapClient = mailtrapClient;
            _settings = settings.Value;
        }



        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var request = SendEmailRequest.Create()
                .From(_settings.SenderEmail, _settings.SenderName)
                .To(to)
                .Subject(subject)
                .Html(body);

            var response = await _mailtrapClient.Test(_settings.InboxId).Send(request);

            if (!response.Success)
            {
                throw new Exception($"Mailtrap Error: {string.Join(", ", response.MessageIds)}");
            }


        }


        public async Task SendEmailWithAttachmentAsync(
            string to,
            string subject,
            string body,
            string pdfBase64,
            string pdfFileName )
        {
            var request = SendEmailRequest.Create()
                .From(_settings.SenderEmail, _settings.SenderName)
                .To(to)
                .Subject(subject)
                .Html(body);

            request.Attachments = new List<Mailtrap.Emails.Models.Attachment> {
                                  new Mailtrap.Emails.Models.Attachment(
                                        pdfBase64,pdfFileName, null,"application/pdf" )};


            var response = await _mailtrapClient.Test(_settings.InboxId).Send(request);

            if (!response.Success)
            {
                throw new Exception($"Mailtrap Error: {string.Join(", ", response.MessageIds)}");
            }
        }
    }

}

