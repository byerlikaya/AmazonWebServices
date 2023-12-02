namespace AmazonWebServices.Services;

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
        var sendRequest = new Amazon.SimpleEmail.Model.SendEmailRequest
        {
            Source = sendEmailRequest.Source,
            Destination = new Amazon.SimpleEmail.Model.Destination
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

    public Task SendEMail(Amazon.SimpleEmail.Model.SendEmailRequest sendEmailRequest)
    {
        throw new NotImplementedException();
    }

    public async Task SendSmtpEMail(Requests.SendSmtpEmailRequest sendEmailRequest)
    {
        var client = new SmtpClient
        {
            Port = 587,
            EnableSsl = true,
            Host = _amazonSesOptions.SmtpOptions.Host,
            Credentials = new NetworkCredential
            {
                UserName = _amazonSesOptions.SmtpOptions.Username,
                Password = _amazonSesOptions.SmtpOptions.Password
            }
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