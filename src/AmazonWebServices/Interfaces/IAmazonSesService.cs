using AmazonWebServices.Requests;
using System.Threading.Tasks;

namespace AmazonWebServices.Interfaces
{
    public interface IAmazonSesService
    {
        Task SendEMail(SendEmailRequest sendEmailRequest);

        Task SendSmtpEMail(SendSmtpEmailRequest sendEmailRequest);
    }
}
