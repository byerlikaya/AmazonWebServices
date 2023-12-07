namespace AmazonWebServices.Interfaces;

public interface IAmazonSesService
{
    Task SendEMail(Requests.SendEmailRequest sendEmailRequest);

    Task SendSmtpEMail(SendSmtpEmailRequest sendEmailRequest);
}