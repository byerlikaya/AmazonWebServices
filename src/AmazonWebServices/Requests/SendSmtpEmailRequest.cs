namespace AmazonWebServices.Requests;

public class SendSmtpEmailRequest : SendEmailRequest
{
    public AttachmentCollection Attachments { get; set; } = new MailMessage().Attachments;
}