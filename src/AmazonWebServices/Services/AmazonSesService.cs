using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using AmazonWebServices.Interfaces;
using AmazonWebServices.Options;
using AmazonWebServices.Utilities;
using CsQuery.ExtensionMethods.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AmazonWebServices.Services
{
    internal class AmazonSesService : IAmazonSesService
    {
        private readonly AmazonCredentialOptions _awsCredential;
        private readonly AmazonSESOptions _amazonSesOptions;

        public AmazonSesService()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _awsCredential = configuration.GetSection(nameof(AmazonCredentialOptions)).Get<AmazonCredentialOptions>();
            _amazonSesOptions = configuration.GetSection(nameof(AmazonSESOptions)).Get<AmazonSESOptions>();
        }

        public async Task SendEMail(Requests.SendEmailRequest sendEmailRequest)
        {
            AWSCredentials credentials = new BasicAWSCredentials(_awsCredential.AccessKey, _awsCredential.SecretKey);

            using var client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.EUCentral1);
            var sendRequest = new SendEmailRequest
            {
                Source = sendEmailRequest.Source,
                Destination = new Destination
                {
                    ToAddresses = sendEmailRequest.Destination.ToAddresses,
                    BccAddresses = sendEmailRequest.Destination.BccAddresses,
                    CcAddresses = sendEmailRequest.Destination.CcAddresses
                },
                Message = new Message
                {
                    Subject = new Content(sendEmailRequest.Subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = sendEmailRequest.Body
                        }
                    }
                },
                ConfigurationSetName = _amazonSesOptions.ConfigurationSetName
            };

            await client.SendEmailAsync(sendRequest);
        }

        public async Task SendSmtpEMail(Requests.SendSmtpEmailRequest sendEmailRequest)
        {
            var networkCredential = new NetworkCredential
            {
                UserName = _amazonSesOptions.SmtpOptions.Username,
                Password = _amazonSesOptions.SmtpOptions.Password
            };

            var client = new SmtpClient
            {
                Port = 587,
                EnableSsl = true,
                Host = _amazonSesOptions.SmtpOptions.Host,
                Credentials = networkCredential
            };

            var mailMessage = new MailMessage
            {
                Subject = sendEmailRequest.Subject,
                Body = sendEmailRequest.Body,
                IsBodyHtml = true,
                From = new MailAddress(sendEmailRequest.Source, sendEmailRequest.DisplayName)
            };

            sendEmailRequest.Destination.ToAddresses.ForEach(email => mailMessage.To.Add(new MailAddress(email)));
            sendEmailRequest.Destination.BccAddresses.ForEach(email => mailMessage.Bcc.Add(new MailAddress(email)));
            sendEmailRequest.Destination.CcAddresses.ForEach(email => mailMessage.CC.Add(new MailAddress(email)));

            mailMessage.Headers.Add("X-SES-CONFIGURATION-SET", _amazonSesOptions.ConfigurationSetName);

            if (sendEmailRequest.Attachments.Any())
                mailMessage.Attachments.AddRange(sendEmailRequest.Attachments);

            await client.SendMailAsync(mailMessage);
        }
    }
}
