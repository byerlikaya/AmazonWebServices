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
            await _amazonSesService.SendEMail(sendEmailRequest);

            return Ok();
        }

        [Consumes("multipart/form-data")]
        [Produces("multipart/form-data", "application/json")]
        [HttpPost("/sendSmtpEmail")]
        public async Task<IActionResult> SendSmtpEmail(SendSmtpEmailRequest sendSmtpEmailRequest)
        {
            await _amazonSesService.SendSmtpEMail(sendSmtpEmailRequest);

            return Ok();
        }
    }
}