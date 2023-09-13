# Amazon Web Services
#### It offers simple yet effective methods for Amazon S3 and Simple Email Service.

![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/byerlikaya/AmazonWebServices/dotnet.yml)
[![AmazonWebServices Nuget](https://img.shields.io/nuget/v/AmazonWebServices)](https://www.nuget.org/packages/AmazonWebServices)
[![AmazonWebServices Nuget](https://img.shields.io/nuget/dt/AmazonWebServices)](https://www.nuget.org/packages/AmazonWebServices)

#### Quick Start
The usage of **AmazonWebServices** is quite simple.

1. Install `AmazonWebServices` NuGet package from [here](https://www.nuget.org/packages/AmazonWebServices/).

````
PM> Install-Package AmazonWebServices
````

2. Add services.AddAmazonWebServices();

```csharp
builder.Services.AddAmazonWebServices();
```

3. Add the necessary information to the `appsettings.json` file.

```json
 "AmazonCredentialOptions": {
    "AccessKey": "",
    "SecretKey": ""
  },

  "AmazonS3Options": {
    "BucketName": ""
  },

  "AmazonSESOptions": {
    "ConfigurationSetName": "",
    "SmtpOptions": {
      "Host": "",
      "UserName": "",
      "Password": ""
    }
  }
```

4. And start using it. And that's it.

`To upload and delete files to Amazon S3. It currently supports these extensions: ".jpeg", ".jpg", ".png", ".pdf", ".svg".`

```csharp
[ApiController]
public class AmazonS3Service : ControllerBase
{
    private readonly IAmazonS3Service _amazonS3Service;

    public AmazonS3Service(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    [Consumes("multipart/form-data")]
    [Produces("multipart/form-data", "application/json")]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string folderName, [FromQuery] string fileName)
    {
        var uploadedFileName = await _amazonS3Service.UploadAsync(new UploadObjectRequest
        {
            File = file,
            FileName = fileName,
            FolderName = folderName
        });
        return Ok(uploadedFileName);
    }

    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromQuery] string folderName, [FromQuery] string fileName)
    {
        await _amazonS3Service.DeleteAsync(fileName, folderName);
        return Ok();
    }
}
```

`To send emails with Amazon SES.`
  
```csharp
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
```