namespace AmazonWebServices.Sample.Api.Controllers;

[ApiController]
// ReSharper disable once HollowTypeName
public class AmazonSesServiceController(IAmazonSesService amazonSesService) : ControllerBase
{
    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [HttpPost("/sendEmail")]
    public async Task<IActionResult> SendEmail(SendEmailRequest sendEmailRequest)
    {
        await amazonSesService.SendEMail(sendEmailRequest);

        return Ok();
    }

    [Consumes("multipart/form-data")]
    [Produces("multipart/form-data", "application/json")]
    [HttpPost("/sendSmtpEmail")]
    public async Task<IActionResult> SendSmtpEmail(SendSmtpEmailRequest sendSmtpEmailRequest)
    {
        await amazonSesService.SendSmtpEMail(sendSmtpEmailRequest);

        return Ok();
    }
}