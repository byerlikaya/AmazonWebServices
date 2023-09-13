using AmazonWebServices.Interfaces;
using AmazonWebServices.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AmazonWebServices.Sample.Api.Controllers
{
    [ApiController]
    public class AmazonSesServiceController : ControllerBase
    {
        private readonly IAmazonSesService _amazonSesService;

        public AmazonSesServiceController(IAmazonSesService amazonSesService)
        {
            _amazonSesService = amazonSesService;
        }


        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [HttpPost("/sendEmail")]
        public async Task<IActionResult> SendEmail(SendEmailRequest sendEmailRequest)
        {
            await _amazonSesService.SendEMail(new SendEmailRequest
            {
                Subject = sendEmailRequest.Subject,
                Body = sendEmailRequest.Body,
                Destination = new Destination
                {
                    BccAddresses = sendEmailRequest.Destination.BccAddresses,
                    CcAddresses = sendEmailRequest.Destination.CcAddresses,
                    ToAddresses = sendEmailRequest.Destination.ToAddresses
                },
                DisplayName = sendEmailRequest.DisplayName,
                Source = sendEmailRequest.Source
            });

            return Ok();
        }

        [Consumes("multipart/form-data")]
        [Produces("multipart/form-data", "application/json")]
        [HttpPost("/sendSmtpEmail")]
        public async Task<IActionResult> SendSmtpEmail(SendSmtpEmailRequest sendEmailRequest)
        {
            await _amazonSesService.SendSmtpEMail(new SendSmtpEmailRequest
            {
                Subject = sendEmailRequest.Subject,
                Body = sendEmailRequest.Body,
                Destination = new Destination
                {
                    BccAddresses = sendEmailRequest.Destination.BccAddresses,
                    CcAddresses = sendEmailRequest.Destination.CcAddresses,
                    ToAddresses = sendEmailRequest.Destination.ToAddresses
                },
                DisplayName = sendEmailRequest.DisplayName,
                Source = sendEmailRequest.Source,
                Attachments = sendEmailRequest.Attachments
            });

            return Ok();
        }
    }
}