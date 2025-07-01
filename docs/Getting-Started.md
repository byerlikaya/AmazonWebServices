# AmazonWebServices Kütüphanesi - Başlangıç Rehberi

## 1. Ön Gereksinimler

### AWS Hesabı ve Ayarları

1. **AWS Hesabı**: Aktif bir AWS hesabınızın olması gerekir
2. **IAM Kullanıcısı**: S3 ve SES için gerekli izinlere sahip IAM kullanıcısı
3. **Access Keys**: Access Key ID ve Secret Access Key
4. **S3 Bucket**: Dosya yükleme için S3 bucket'ı
5. **SES Doğrulaması**: E-posta gönderimi için SES domain/e-posta doğrulaması

### Geliştirme Ortamı

- **.NET 6.0** veya üzeri (NetStandard 2.0 destekli)
- **Visual Studio 2022** veya **VS Code**
- **NuGet Package Manager**

## 2. Kurulum Adımları

### Adım 1: NuGet Paketi Kurulumu

**Package Manager Console ile:**
```bash
Install-Package AmazonWebServices
```

**PackageReference ile (.csproj dosyasına ekleyin):**
```xml
<PackageReference Include="AmazonWebServices" Version="2.1.4.1" />
```

**.NET CLI ile:**
```bash
dotnet add package AmazonWebServices
```

### Adım 2: Service Registration

`Program.cs` dosyanızda servisleri kaydedin:

```csharp
using AmazonWebServices.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Diğer servisler
builder.Services.AddControllers();

// AmazonWebServices servislerini ekle
builder.Services.AddAmazonWebServices();

var app = builder.Build();

// Pipeline konfigürasyonu
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Adım 3: Konfigürasyon Ayarları

`appsettings.json` dosyanızı düzenleyin:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  
  "AmazonCredentialOptions": {
    "AccessKey": "AKIAI44QH8DHBEXAMPLE",
    "SecretKey": "je7MtGbClwBF/2Zp9Utk/h3yCo8nvbEXAMPLEKEY"
  },
  
  "AmazonS3Options": {
    "BucketName": "my-application-bucket",
    "Region": "eu-central-1"
  },
  
  "AmazonSESOptions": {
    "ConfigurationSetName": "my-configuration-set",
    "SmtpOptions": {
      "Host": "email-smtp.eu-central-1.amazonaws.com",
      "UserName": "AKIAI44QH8DHBEXAMPLE",
      "Password": "BLueSKY1987+example"
    }
  }
}
```

> ⚠️ **Güvenlik Notu**: Üretim ortamında hassas bilgileri environment variables, Azure Key Vault veya AWS Secrets Manager kullanarak saklayın.

## 3. İlk S3 Controller'ınızı Oluşturun

### Basit Dosya Yükleme Controller'ı

```csharp
using Microsoft.AspNetCore.Mvc;
using AmazonWebServices.Interfaces;
using AmazonWebServices.Requests;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly ILogger<FileController> _logger;

    public FileController(
        IAmazonS3Service amazonS3Service, 
        ILogger<FileController> logger)
    {
        _amazonS3Service = amazonS3Service;
        _logger = logger;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile(
        [FromForm] IFormFile file,
        [FromQuery] string? folderName = null,
        [FromQuery] string? fileName = null)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Lütfen bir dosya seçin.");
            }

            var request = new UploadObjectRequest
            {
                File = file,
                FileName = fileName ?? file.FileName,
                FolderName = folderName ?? "uploads"
            };

            var uploadedFileName = await _amazonS3Service.UploadAsync(request);

            _logger.LogInformation("Dosya başarıyla yüklendi: {FileName}", uploadedFileName);

            return Ok(new { 
                Success = true, 
                FileName = uploadedFileName,
                Message = "Dosya başarıyla yüklendi." 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dosya yükleme hatası");
            return StatusCode(500, new { 
                Success = false, 
                Message = "Dosya yükleme sırasında hata oluştu." 
            });
        }
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeleteFile(
        string fileName,
        [FromQuery] string? folderName = null)
    {
        try
        {
            await _amazonS3Service.DeleteAsync(fileName, folderName);
            
            _logger.LogInformation("Dosya başarıyla silindi: {FileName}", fileName);
            
            return Ok(new { 
                Success = true, 
                Message = "Dosya başarıyla silindi." 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dosya silme hatası: {FileName}", fileName);
            return StatusCode(500, new { 
                Success = false, 
                Message = "Dosya silme sırasında hata oluştu." 
            });
        }
    }
}
```

## 4. İlk E-posta Controller'ınızı Oluşturun

### Basit E-posta Gönderme Controller'ı

```csharp
using Microsoft.AspNetCore.Mvc;
using AmazonWebServices.Interfaces;
using AmazonWebServices.Requests;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IAmazonSesService _amazonSesService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(
        IAmazonSesService amazonSesService,
        ILogger<EmailController> logger)
    {
        _amazonSesService = amazonSesService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        try
        {
            var emailRequest = new SendEmailRequest
            {
                DisplayName = "My Application",
                Source = "noreply@myapp.com",
                Subject = request.Subject,
                Body = request.Body,
                Destination = new Destination
                {
                    ToAddresses = request.ToEmails,
                    CcAddresses = request.CcEmails ?? new List<string>(),
                    BccAddresses = request.BccEmails ?? new List<string>()
                }
            };

            await _amazonSesService.SendEMail(emailRequest);

            _logger.LogInformation("E-posta başarıyla gönderildi: {Subject}", request.Subject);

            return Ok(new { 
                Success = true, 
                Message = "E-posta başarıyla gönderildi." 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "E-posta gönderme hatası");
            return StatusCode(500, new { 
                Success = false, 
                Message = "E-posta gönderme sırasında hata oluştu." 
            });
        }
    }

    [HttpPost("send-smtp")]
    public async Task<IActionResult> SendSmtpEmail([FromBody] SmtpEmailRequest request)
    {
        try
        {
            var smtpRequest = new SendSmtpEmailRequest
            {
                From = request.From,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = request.IsBodyHtml
            };

            await _amazonSesService.SendSmtpEMail(smtpRequest);

            _logger.LogInformation("SMTP e-posta başarıyla gönderildi: {Subject}", request.Subject);

            return Ok(new { 
                Success = true, 
                Message = "E-posta başarıyla gönderildi." 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SMTP e-posta gönderme hatası");
            return StatusCode(500, new { 
                Success = false, 
                Message = "E-posta gönderme sırasında hata oluştu." 
            });
        }
    }
}

// Request modelleri
public class EmailRequest
{
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public required List<string> ToEmails { get; set; }
    public List<string>? CcEmails { get; set; }
    public List<string>? BccEmails { get; set; }
}

public class SmtpEmailRequest
{
    public required string From { get; set; }
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public bool IsBodyHtml { get; set; } = true;
}
```

## 5. AWS Ayarları

### IAM Kullanıcısı Oluşturma

1. **AWS Console**'a giriş yapın
2. **IAM** servisine gidin
3. **Users** > **Create User** tıklayın
4. Kullanıcı adını girin
5. **Attach policies directly** seçin
6. Aşağıdaki policy'leri ekleyin:
   - `AmazonS3FullAccess` (veya özel S3 policy)
   - `AmazonSESFullAccess` (veya özel SES policy)

### Özel IAM Policy Örneği

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "s3:GetObject",
                "s3:PutObject",
                "s3:DeleteObject"
            ],
            "Resource": "arn:aws:s3:::your-bucket-name/*"
        },
        {
            "Effect": "Allow",
            "Action": [
                "s3:ListBucket"
            ],
            "Resource": "arn:aws:s3:::your-bucket-name"
        },
        {
            "Effect": "Allow",
            "Action": [
                "ses:SendEmail",
                "ses:SendRawEmail"
            ],
            "Resource": "*"
        }
    ]
}
```

### S3 Bucket Oluşturma

1. **S3** servisine gidin
2. **Create bucket** tıklayın
3. Bucket adını girin (benzersiz olmalı)
4. Region seçin
5. **Block all public access** ayarını ihtiyacınıza göre düzenleyin
6. **Create bucket** tıklayın

### SES Ayarları

1. **SES** servisine gidin
2. **Verified identities** > **Create identity** tıklayın
3. **Domain** veya **Email address** seçin
4. Doğrulama adımlarını tamamlayın
5. **Configuration sets** oluşturun (opsiyonel)

## 6. Test Etme

### Swagger/OpenAPI ile Test

`Program.cs` dosyanıza Swagger ekleyin:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAmazonWebServices();

// Swagger ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development ortamında Swagger'ı etkinleştir
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Postman ile Test

**Dosya Yükleme Testi:**
```
POST https://localhost:7000/api/file/upload
Content-Type: multipart/form-data

form-data:
- file: [dosya seçin]
- folderName: "test-uploads"
- fileName: "test-file.jpg"
```

**E-posta Gönderme Testi:**
```
POST https://localhost:7000/api/email/send
Content-Type: application/json

{
  "subject": "Test E-posta",
  "body": "<h1>Merhaba!</h1><p>Bu bir test e-postasıdır.</p>",
  "toEmails": ["test@example.com"]
}
```

## 7. Yaygın Hatalar ve Çözümleri

### 1. Access Denied Hatası

**Problem**: `AccessDenied` hatası alıyorsunuz.

**Çözüm**: 
- IAM kullanıcısının gerekli izinlere sahip olduğunu kontrol edin
- Access Key ve Secret Key'in doğru olduğunu kontrol edin

### 2. Bucket Not Found Hatası

**Problem**: `NoSuchBucket` hatası alıyorsunuz.

**Çözüm**: 
- Bucket adının doğru olduğunu kontrol edin
- Bucket'ın belirtilen region'da olduğunu kontrol edin

### 3. SES Verification Hatası

**Problem**: E-posta gönderemiyorsunuz.

**Çözüm**: 
- Gönderen e-posta adresinin SES'te doğrulandığını kontrol edin
- SES sandbox modundan çıktığınızı kontrol edin

### 4. File Extension Hatası

**Problem**: Dosya yüklenmiyor.

**Çözüm**: 
- Desteklenen dosya türlerini kontrol edin: `.jpeg`, `.jpg`, `.png`, `.pdf`, `.svg`, `.webp`

## 8. Environment Variables ile Güvenlik

### appsettings.json'dan Environment Variables'a Geçiş

**Adım 1**: Environment variables tanımlayın
```bash
export AWS_ACCESS_KEY_ID="your-access-key"
export AWS_SECRET_ACCESS_KEY="your-secret-key"
export AWS_S3_BUCKET_NAME="your-bucket-name"
export AWS_REGION="eu-central-1"
```

**Adım 2**: appsettings.json'ı güncelleyin
```json
{
  "AmazonCredentialOptions": {
    "AccessKey": "${AWS_ACCESS_KEY_ID}",
    "SecretKey": "${AWS_SECRET_ACCESS_KEY}"
  },
  "AmazonS3Options": {
    "BucketName": "${AWS_S3_BUCKET_NAME}",
    "Region": "${AWS_REGION}"
  }
}
```

## 9. Production'a Hazırlık

### Logging Konfigürasyonu

```csharp
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
    // Production'da structured logging kullanın (Serilog, NLog)
});
```

### Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddCheck<S3HealthCheck>("s3")
    .AddCheck<SESHealthCheck>("ses");

app.MapHealthChecks("/health");
```

### Rate Limiting

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FileUpload", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
    });
});
```

## 10. Sonraki Adımlar

1. **Gelişmiş Özellikler**: [API-Documentation.md](./API-Documentation.md) dosyasını inceleyin
2. **Sample Proje**: `sample/AmazonWebServices.Sample.Api` projesini çalıştırın
3. **Error Handling**: Özel exception handling implementasyonu ekleyin
4. **Testing**: Unit test ve integration test yazın
5. **Monitoring**: Application Insights veya CloudWatch entegrasyonu

Bu rehberi takip ederek AmazonWebServices kütüphanesini projenizde başarıyla kullanmaya başlayabilirsiniz!