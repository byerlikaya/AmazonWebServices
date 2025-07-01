# AmazonWebServices Kütüphanesi Dokümantasyonu

## Genel Bakış

AmazonWebServices, Amazon S3 ve Amazon Simple Email Service (SES) için basit ve etkili metodlar sunan bir .NET kütüphanesidir. Bu kütüphane, AWS hizmetlerini .NET uygulamalarınızda kolayca kullanmanızı sağlar.

## Özellikler

- **Amazon S3 Desteği**: Dosya yükleme, silme ve yönetimi
- **Amazon SES Desteği**: E-posta gönderimi (hem SES API hem de SMTP)
- **Kolay Kurulum**: Tek satırda DI konteynırına entegrasyon
- **Esnek Konfigürasyon**: JSON tabanlı ayar yönetimi
- **.NET Standard 2.0 Desteği**: Geniş platform uyumluluğu

## Kurulum

### NuGet Paketi ile Kurulum

```bash
PM> Install-Package AmazonWebServices
```

veya .NET CLI ile:

```bash
dotnet add package AmazonWebServices
```

## Konfigürasyon

### 1. Dependency Injection Kaydı

`Program.cs` veya `Startup.cs` dosyanızda:

```csharp
builder.Services.AddAmazonWebServices();
```

### 2. Ayarlar Dosyası (appsettings.json)

```json
{
  "AmazonCredentialOptions": {
    "AccessKey": "your-access-key",
    "SecretKey": "your-secret-key"
  },
  "AmazonS3Options": {
    "BucketName": "your-bucket-name",
    "Region": "eu-central-1"
  },
  "AmazonSESOptions": {
    "ConfigurationSetName": "your-configuration-set",
    "SmtpOptions": {
      "Host": "email-smtp.eu-central-1.amazonaws.com",
      "UserName": "your-smtp-username",
      "Password": "your-smtp-password"
    }
  }
}
```

## Amazon S3 Servisi Kullanımı

### Desteklenen Dosya Türleri

S3 servisi aşağıdaki dosya uzantılarını destekler:
- `.jpeg`, `.jpg`, `.png`, `.pdf`, `.svg`, `.webp`

### 1. Dosya Yükleme (IFormFile ile)

```csharp
[ApiController]
public class FileController : ControllerBase
{
    private readonly IAmazonS3Service _amazonS3Service;

    public FileController(IAmazonS3Service amazonS3Service)
    {
        _amazonS3Service = amazonS3Service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, 
                                               [FromQuery] string folderName, 
                                               [FromQuery] string fileName)
    {
        var uploadedFileName = await _amazonS3Service.UploadAsync(new UploadObjectRequest
        {
            File = file,
            FileName = fileName,
            FolderName = folderName
        });
        
        return Ok(uploadedFileName);
    }
}
```

### 2. Dosya Yükleme (Dosya Yolu ile)

```csharp
[HttpPost("upload-path")]
public async Task<IActionResult> UploadFile([FromQuery] string filePath, 
                                           [FromQuery] string folderName, 
                                           [FromQuery] string fileName)
{
    var uploadedFileName = await _amazonS3Service.UploadAsync(new UploadRequest
    {
        FilePath = filePath,
        FileName = fileName,
        FolderName = folderName
    });
    
    return Ok(uploadedFileName);
}
```

### 3. Dosya Silme

```csharp
[HttpDelete("delete")]
public async Task<IActionResult> DeleteFile([FromQuery] string fileName, 
                                           [FromQuery] string folderName)
{
    await _amazonS3Service.DeleteAsync(fileName, folderName);
    return Ok();
}
```

### 4. Harici AWS Hesabına Yükleme

```csharp
[HttpPost("upload-external")]
public async Task<IActionResult> UploadToExternalAws([FromBody] UploadExternalAwsRequest request)
{
    var uploadedFileName = await _amazonS3Service.UploadExternalAwsAsync(request);
    return Ok(uploadedFileName);
}
```

## Amazon SES Servisi Kullanımı

### 1. SES API ile E-posta Gönderimi

```csharp
[ApiController]
public class EmailController : ControllerBase
{
    private readonly IAmazonSesService _amazonSesService;

    public EmailController(IAmazonSesService amazonSesService)
    {
        _amazonSesService = amazonSesService;
    }

    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
    {
        await _amazonSesService.SendEMail(request);
        return Ok();
    }
}
```

### SendEmailRequest Modeli

```csharp
var emailRequest = new SendEmailRequest
{
    DisplayName = "Şirket Adı",
    Source = "noreply@example.com",
    Subject = "E-posta Konusu",
    Body = "<html><body><h1>Merhaba!</h1><p>Bu bir test e-postasıdır.</p></body></html>",
    Destination = new Destination
    {
        ToAddresses = new List<string> { "user@example.com" },
        CcAddresses = new List<string> { "cc@example.com" },
        BccAddresses = new List<string> { "bcc@example.com" }
    }
};
```

### 2. SMTP ile E-posta Gönderimi

```csharp
[HttpPost("send-smtp-email")]
public async Task<IActionResult> SendSmtpEmail([FromBody] SendSmtpEmailRequest request)
{
    await _amazonSesService.SendSmtpEMail(request);
    return Ok();
}
```

## Request Modelleri

### UploadObjectRequest
```csharp
public class UploadObjectRequest
{
    public IFormFile File { get; set; }
    public string FileName { get; set; }
    public string FolderName { get; set; }
}
```

### UploadRequest
```csharp
public class UploadRequest
{
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string FolderName { get; set; }
}
```

### UploadExternalAwsRequest
```csharp
public class UploadExternalAwsRequest
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public string Region { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string FolderName { get; set; }
}
```

### SendEmailRequest
```csharp
public class SendEmailRequest
{
    public string DisplayName { get; set; }
    public string Source { get; set; }
    public Destination Destination { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class Destination
{
    public List<string> ToAddresses { get; set; }
    public List<string> BccAddresses { get; set; }
    public List<string> CcAddresses { get; set; }
}
```

### SendSmtpEmailRequest
```csharp
public class SendSmtpEmailRequest
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
}
```

## Konfigürasyon Seçenekleri

### AmazonCredentialOptions
```csharp
public class AmazonCredentialOptions
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}
```

### AmazonS3Options
```csharp
public class AmazonS3Options
{
    public string BucketName { get; set; }
    public string Region { get; set; }
}
```

### AmazonSESOptions
```csharp
public class AmazonSESOptions
{
    public string ConfigurationSetName { get; set; }
    public SmtpOptions SmtpOptions { get; set; }
}

public class SmtpOptions
{
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
```

## Güvenlik Notları

1. **Kimlik Bilgileri**: AWS kimlik bilgilerinizi güvenli bir şekilde saklayın
2. **IAM Rolleri**: Minimum gerekli izinlerle IAM rolleri kullanın
3. **S3 Bucket Politikaları**: Bucket'larınızı uygun güvenlik politikalarıyla koruyın
4. **SES Ayarları**: E-posta gönderimi için SES doğrulamasını tamamlayın

## Hata Yönetimi

Tüm async metodlar exception fırlatabilir. Uygun try-catch blokları kullanmayı unutmayın:

```csharp
try
{
    var result = await _amazonS3Service.UploadAsync(request);
    // Başarılı işlem
}
catch (AmazonS3Exception ex)
{
    // S3 özel hatası
    _logger.LogError(ex, "S3 hatası oluştu");
}
catch (Exception ex)
{
    // Genel hata
    _logger.LogError(ex, "Beklenmeyen hata oluştu");
}
```

## Sample Proje

`sample/AmazonWebServices.Sample.Api` dizininde tam bir örnek proje bulabilirsiniz. Bu proje, kütüphanenin nasıl kullanılacağını gösterir.

## Bağımlılıklar

- AWSSDK.S3: ^3.7.416.5
- AWSSDK.SimpleEmail: ^3.7.402.72
- CsQuery.NETStandard: ^1.3.6.1
- Microsoft.AspNetCore.Http: ^2.3.0
- Microsoft.Extensions.Configuration.Binder: ^9.0.3
- Microsoft.Extensions.DependencyInjection: ^9.0.3

## Sürüm Bilgileri

- **Mevcut Sürüm**: 2.1.4.1
- **Hedef Framework**: .NET Standard 2.0
- **Lisans**: [LICENSE](../LICENSE) dosyasını inceleyin

## Katkıda Bulunma

Projeye katkıda bulunmak için [GitHub deposunu](https://github.com/byerlikaya/AmazonWebServices) ziyaret edin.

## Destek

Sorularınız için GitHub Issues kullanabilir veya proje sahibi ile iletişime geçebilirsiniz.