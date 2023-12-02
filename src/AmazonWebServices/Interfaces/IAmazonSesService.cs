namespace AmazonWebServices.Interfaces;

public interface IAmazonSesService
{
    Task SendEMail(Amazon.SimpleEmail.Model.SendEmailRequest sendEmailRequest);

    Task SendSmtpEMail(SendSmtpEmailRequest sendEmailRequest);
}