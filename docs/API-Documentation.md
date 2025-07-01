# AmazonWebServices API Dokümantasyonu

## İçindekiler

1. [Amazon S3 Service API](#amazon-s3-service-api)
2. [Amazon SES Service API](#amazon-ses-service-api)
3. [Request/Response Modelleri](#requestresponse-modelleri)
4. [Hata Kodları](#hata-kodları)
5. [Örnek Kullanımlar](#örnek-kullanımlar)

## Amazon S3 Service API

### IAmazonS3Service Interface

Amazon S3 işlemleri için ana arayüz.

#### Metodlar

##### UploadAsync(UploadObjectRequest)

**Açıklama**: IFormFile ile dosya yükleme

**Parametre**: `UploadObjectRequest updaObjectRequest`

**Dönüş Değeri**: `Task<string>` - Yüklenen dosyanın adı

**Desteklenen Dosya Formatları**: `.jpeg`, `.jpg`, `.png`, `.pdf`, `.svg`, `.webp`

```csharp
public async Task<string> UploadAsync(UploadObjectRequest updaObjectRequest)
```

**Örnek**:
```csharp
var result = await _amazonS3Service.UploadAsync(new UploadObjectRequest
{
    File = formFile,
    FileName = "profile-image.jpg",
    FolderName = "users/images"
});
```

---

##### UploadAsync(UploadRequest)

**Açıklama**: Dosya yolu ile dosya yükleme

**Parametre**: `UploadRequest uploadRequest`

**Dönüş Değeri**: `Task<string>` - Yüklenen dosyanın adı

```csharp
public async Task<string> UploadAsync(UploadRequest uploadRequest)
```

**Örnek**:
```csharp
var result = await _amazonS3Service.UploadAsync(new UploadRequest
{
    FilePath = @"C:\temp\document.pdf",
    FileName = "important-document.pdf",
    FolderName = "documents"
});
```

---

##### UploadExternalAwsAsync(UploadExternalAwsRequest)

**Açıklama**: Farklı AWS hesabına dosya yükleme

**Parametre**: `UploadExternalAwsRequest uploadRequest`

**Dönüş Değeri**: `Task<string>` - Yüklenen dosyanın adı

```csharp
public async Task<string> UploadExternalAwsAsync(UploadExternalAwsRequest uploadRequest)
```

**Örnek**:
```csharp
var result = await _amazonS3Service.UploadExternalAwsAsync(new UploadExternalAwsRequest
{
    AccessKey = "external-access-key",
    SecretKey = "external-secret-key",
    BucketName = "external-bucket",
    Region = "us-east-1",
    FilePath = @"C:\temp\backup.zip",
    FileName = "backup.zip",
    FolderName = "backups"
});
```

---

##### DeleteAsync(string, string)

**Açıklama**: S3'ten dosya silme

**Parametreler**: 
- `string fileName` - Silinecek dosyanın adı
- `string folderName` - Klasör adı (opsiyonel)

**Dönüş Değeri**: `Task`

```csharp
public async Task DeleteAsync(string fileName, string folderName = null)
```

**Örnek**:
```csharp
await _amazonS3Service.DeleteAsync("old-document.pdf", "documents");
```

## Amazon SES Service API

### IAmazonSesService Interface

Amazon SES e-posta işlemleri için ana arayüz.

#### Metodlar

##### SendEMail(SendEmailRequest)

**Açıklama**: Amazon SES API ile e-posta gönderimi

**Parametre**: `SendEmailRequest sendEmailRequest`

**Dönüş Değeri**: `Task`

```csharp
public async Task SendEMail(SendEmailRequest sendEmailRequest)
```

**Örnek**:
```csharp
await _amazonSesService.SendEMail(new SendEmailRequest
{
    DisplayName = "My Company",
    Source = "noreply@mycompany.com",
    Subject = "Welcome Email",
    Body = "<h1>Welcome!</h1><p>Thank you for joining us.</p>",
    Destination = new Destination
    {
        ToAddresses = new List<string> { "user@example.com" },
        CcAddresses = new List<string> { "manager@example.com" },
        BccAddresses = new List<string> { "archive@example.com" }
    }
});
```

---

##### SendSmtpEMail(SendSmtpEmailRequest)

**Açıklama**: SMTP ile e-posta gönderimi

**Parametre**: `SendSmtpEmailRequest sendEmailRequest`

**Dönüş Değeri**: `Task`

```csharp
public async Task SendSmtpEMail(SendSmtpEmailRequest sendEmailRequest)
```

**Örnek**:
```csharp
await _amazonSesService.SendSmtpEMail(new SendSmtpEmailRequest
{
    From = "sender@mycompany.com",
    To = "recipient@example.com",
    Subject = "SMTP Test Email",
    Body = "<h1>Test</h1><p>This is a test email via SMTP.</p>",
    IsBodyHtml = true
});
```

## Request/Response Modelleri

### S3 Request Modelleri

#### UploadObjectRequest

```csharp
public class UploadObjectRequest
{
    public IFormFile File { get; set; }       // Yüklenecek dosya
    public string FileName { get; set; }      // Dosya adı
    public string FolderName { get; set; }    // Klasör adı (opsiyonel)
}
```

#### UploadRequest

```csharp
public class UploadRequest
{
    public string FilePath { get; set; }      // Dosya yolu
    public string FileName { get; set; }      // Dosya adı
    public string FolderName { get; set; }    // Klasör adı (opsiyonel)
}
```

#### UploadExternalAwsRequest

```csharp
public class UploadExternalAwsRequest
{
    public string AccessKey { get; set; }     // AWS Access Key
    public string SecretKey { get; set; }     // AWS Secret Key
    public string BucketName { get; set; }    // Bucket adı
    public string Region { get; set; }        // AWS Region
    public string FilePath { get; set; }      // Dosya yolu
    public string FileName { get; set; }      // Dosya adı
    public string FolderName { get; set; }    // Klasör adı (opsiyonel)
}
```

### SES Request Modelleri

#### SendEmailRequest

```csharp
public class SendEmailRequest
{
    public string DisplayName { get; set; }   // Görünen ad
    public string Source { get; set; }        // Gönderen e-posta
    public Destination Destination { get; set; } // Alıcı bilgileri
    public string Subject { get; set; }       // E-posta konusu
    public string Body { get; set; }          // E-posta içeriği (HTML)
}
```

#### Destination

```csharp
public class Destination
{
    public List<string> ToAddresses { get; set; }  // Ana alıcılar
    public List<string> BccAddresses { get; set; } // Gizli alıcılar
    public List<string> CcAddresses { get; set; }  // Kopyası gönderilecekler
}
```

#### SendSmtpEmailRequest

```csharp
public class SendSmtpEmailRequest
{
    public string From { get; set; }          // Gönderen
    public string To { get; set; }            // Alıcı
    public string Subject { get; set; }       // Konu
    public string Body { get; set; }          // İçerik
    public bool IsBodyHtml { get; set; }      // HTML formatı
}
```

## Hata Kodları

### S3 Hataları

| Hata Kodu | Açıklama | Çözüm |
|-----------|----------|-------|
| `NoSuchBucket` | Bucket bulunamadı | Bucket adını kontrol edin |
| `AccessDenied` | Erişim reddedildi | IAM izinlerini kontrol edin |
| `InvalidRequest` | Geçersiz istek | Request parametrelerini kontrol edin |
| `SignatureDoesNotMatch` | İmza uyumsuzluğu | Access Key/Secret Key'i kontrol edin |

### SES Hataları

| Hata Kodu | Açıklama | Çözüm |
|-----------|----------|-------|
| `MessageRejected` | Mesaj reddedildi | E-posta içeriğini kontrol edin |
| `MailFromDomainNotVerified` | Domain doğrulanmamış | SES'te domain doğrulaması yapın |
| `ConfigurationSetDoesNotExist` | Configuration set bulunamadı | Configuration set adını kontrol edin |
| `SendingPausedException` | Gönderim duraklatılmış | SES hesap durumunu kontrol edin |

## Örnek Kullanımlar

### Tam Controller Örneği

```csharp
[ApiController]
[Route("api/[controller]")]
public class FileManagementController : ControllerBase
{
    private readonly IAmazonS3Service _s3Service;
    private readonly IAmazonSesService _sesService;
    private readonly ILogger<FileManagementController> _logger;

    public FileManagementController(
        IAmazonS3Service s3Service,
        IAmazonSesService sesService,
        ILogger<FileManagementController> logger)
    {
        _s3Service = s3Service;
        _sesService = sesService;
        _logger = logger;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile(
        [FromForm] IFormFile file,
        [FromQuery] string folderName = "uploads",
        [FromQuery] string fileName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            var uploadedFileName = await _s3Service.UploadAsync(new UploadObjectRequest
            {
                File = file,
                FileName = fileName ?? file.FileName,
                FolderName = folderName
            });

            // E-posta bildirimi gönder
            await _sesService.SendEMail(new SendEmailRequest
            {
                DisplayName = "File Service",
                Source = "noreply@company.com",
                Subject = "Dosya Yüklendi",
                Body = $"<p>Dosya başarıyla yüklendi: {uploadedFileName}</p>",
                Destination = new Destination
                {
                    ToAddresses = new List<string> { "admin@company.com" }
                }
            });

            return Ok(new { FileName = uploadedFileName, Message = "Dosya başarıyla yüklendi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dosya yükleme hatası");
            return StatusCode(500, "Dosya yükleme sırasında hata oluştu.");
        }
    }

    [HttpDelete("delete/{fileName}")]
    public async Task<IActionResult> DeleteFile(
        string fileName,
        [FromQuery] string folderName = null)
    {
        try
        {
            await _s3Service.DeleteAsync(fileName, folderName);
            return Ok(new { Message = "Dosya başarıyla silindi." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dosya silme hatası");
            return StatusCode(500, "Dosya silme sırasında hata oluştu.");
        }
    }
}
```

### Gelişmiş E-posta Örneği

```csharp
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IAmazonSesService _sesService;

    public NotificationController(IAmazonSesService sesService)
    {
        _sesService = sesService;
    }

    [HttpPost("send-welcome-email")]
    public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
    {
        var htmlBody = $@"
            <html>
            <body>
                <h1>Hoş Geldiniz, {request.UserName}!</h1>
                <p>Sisteme başarıyla kayıt oldunuz.</p>
                <p>Hesap bilgileriniz:</p>
                <ul>
                    <li>E-posta: {request.Email}</li>
                    <li>Kayıt Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}</li>
                </ul>
                <p>İyi günler!</p>
            </body>
            </html>";

        await _sesService.SendEMail(new SendEmailRequest
        {
            DisplayName = "My Application",
            Source = "welcome@myapp.com",
            Subject = "Hoş Geldiniz!",
            Body = htmlBody,
            Destination = new Destination
            {
                ToAddresses = new List<string> { request.Email }
            }
        });

        return Ok(new { Message = "Hoş geldin e-postası gönderildi." });
    }
}

public class WelcomeEmailRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
```

### Batch İşlemler

```csharp
public class BatchProcessingService
{
    private readonly IAmazonS3Service _s3Service;
    private readonly IAmazonSesService _sesService;

    public BatchProcessingService(IAmazonS3Service s3Service, IAmazonSesService sesService)
    {
        _s3Service = s3Service;
        _sesService = sesService;
    }

    public async Task ProcessMultipleFiles(List<string> filePaths, string targetFolder)
    {
        var uploadedFiles = new List<string>();
        var failedFiles = new List<string>();

        foreach (var filePath in filePaths)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                var uploadedFileName = await _s3Service.UploadAsync(new UploadRequest
                {
                    FilePath = filePath,
                    FileName = fileName,
                    FolderName = targetFolder
                });
                uploadedFiles.Add(uploadedFileName);
            }
            catch (Exception)
            {
                failedFiles.Add(filePath);
            }
        }

        // Rapor e-postası gönder
        var reportBody = $@"
            <h2>Toplu Dosya Yükleme Raporu</h2>
            <p>Başarılı: {uploadedFiles.Count} dosya</p>
            <p>Başarısız: {failedFiles.Count} dosya</p>
            
            {(uploadedFiles.Any() ? $"<h3>Yüklenen Dosyalar:</h3><ul>{string.Join("", uploadedFiles.Select(f => $"<li>{f}</li>"))}</ul>" : "")}
            {(failedFiles.Any() ? $"<h3>Başarısız Dosyalar:</h3><ul>{string.Join("", failedFiles.Select(f => $"<li>{f}</li>"))}</ul>" : "")}
        ";

        await _sesService.SendEMail(new SendEmailRequest
        {
            DisplayName = "Batch Processing Service",
            Source = "batch@myapp.com",
            Subject = "Toplu Dosya Yükleme Raporu",
            Body = reportBody,
            Destination = new Destination
            {
                ToAddresses = new List<string> { "admin@myapp.com" }
            }
        });
    }
}
```

## Performance ve Best Practices

### S3 Optimizasyonları

1. **Paralel Yükleme**: Büyük dosyalar için paralel yükleme kullanın
2. **Dosya Boyutu Kontrolü**: Büyük dosyalar için multipart upload kullanın
3. **Retry Logic**: Geçici hatalar için retry mantığı ekleyin

### SES Optimizasyonları

1. **Rate Limiting**: SES quotalarını aşmamaya dikkat edin
2. **Template Kullanımı**: Tekrarlanan e-postalar için template kullanın
3. **Bounce Handling**: Bounce ve complaint'leri takip edin

### Genel Best Practices

1. **Async/Await**: Tüm işlemleri async olarak gerçekleştirin
2. **Exception Handling**: Uygun hata yönetimi yapın
3. **Logging**: Tüm işlemleri logla
4. **Configuration**: Hassas bilgileri güvenli şekilde saklayın