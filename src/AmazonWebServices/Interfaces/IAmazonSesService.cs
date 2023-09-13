using AmazonWebServices.Requests;

namespace AmazonWebServices.Interfaces
{
    public interface IAmazonSesService
    {
        Task SendEMail(SendEmailRequest sendEmailRequest);

        Task SendSmtpEMail(SendSmtpEmailRequest sendEmailRequest);
    }
}
