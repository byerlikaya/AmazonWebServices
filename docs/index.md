# AmazonWebServices Kütüphanesi - Dokümantasyon

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/byerlikaya/AmazonWebServices/dotnet.yml)
[![AmazonWebServices Nuget](https://img.shields.io/nuget/v/AmazonWebServices)](https://www.nuget.org/packages/AmazonWebServices)
[![AmazonWebServices Nuget](https://img.shields.io/nuget/dt/AmazonWebServices)](https://www.nuget.org/packages/AmazonWebServices)

AmazonWebServices, Amazon S3 ve Amazon Simple Email Service (SES) için basit ve etkili metodlar sunan bir .NET kütüphanesidir. Bu kütüphane, AWS hizmetlerini .NET uygulamalarınızda kolayca kullanmanızı sağlar.

## 📚 Dokümantasyon İçeriği

### 🚀 [Başlangıç Rehberi](./Getting-Started.md)
Kütüphaneyi projenizde nasıl kuracağınız ve ilk adımları nasıl atacağınızı öğrenin:
- Kurulum adımları
- AWS ayarları
- İlk controller'ları oluşturma
- Test etme
- Yaygın hatalar ve çözümleri

### 📖 [Ana Dokümantasyon](./README.md)
Kütüphanenin tüm özelliklerini ve kullanım örneklerini inceleyin:
- Genel bakış ve özellikler
- Konfigürasyon ayarları
- Amazon S3 servisi kullanımı
- Amazon SES servisi kullanımı
- Request/Response modelleri
- Güvenlik notları

### 🔧 [API Dokümantasyonu](./API-Documentation.md)
Detaylı API referansı ve gelişmiş kullanım örnekleri:
- Interface metodları
- Request/Response modelleri
- Hata kodları
- Performance optimizasyonları
- Best practices
- Batch işlemler

## ⚡ Hızlı Başlangıç

### 1. Kurulum
```bash
PM> Install-Package AmazonWebServices
```

### 2. Service Registration
```csharp
builder.Services.AddAmazonWebServices();
```

### 3. Konfigürasyon
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
    "ConfigurationSetName": "your-configuration-set"
  }
}
```

### 4. Kullanım
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
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        var result = await _amazonS3Service.UploadAsync(new UploadObjectRequest
        {
            File = file,
            FileName = file.FileName,
            FolderName = "uploads"
        });
        return Ok(result);
    }
}
```

## 🌟 Ana Özellikler

### Amazon S3 Desteği
- ✅ Dosya yükleme (IFormFile ve dosya yolu)
- ✅ Dosya silme
- ✅ Harici AWS hesabına yükleme
- ✅ Desteklenen formatlar: `.jpeg`, `.jpg`, `.png`, `.pdf`, `.svg`, `.webp`

### Amazon SES Desteği
- ✅ SES API ile e-posta gönderimi
- ✅ SMTP ile e-posta gönderimi
- ✅ HTML e-posta desteği
- ✅ CC/BCC desteği

### Genel Özellikler
- ✅ .NET Standard 2.0 desteği
- ✅ Dependency Injection entegrasyonu
- ✅ Kolay konfigürasyon
- ✅ Async/await desteği
- ✅ Comprehensive error handling

## 📁 Proje Yapısı

```
AmazonWebServices/
├── src/
│   └── AmazonWebServices/
│       ├── Extensions/          # DI extension metodları
│       ├── Interfaces/          # Service interface'leri
│       ├── Options/            # Konfigürasyon sınıfları
│       ├── Requests/           # Request modelleri
│       ├── Services/           # Service implementasyonları
│       └── Utilities/          # Yardımcı sınıflar
├── sample/
│   └── AmazonWebServices.Sample.Api/  # Örnek proje
└── docs/                       # Dokümantasyon dosyaları
```

## 🔗 Faydalı Linkler

- **GitHub Repository**: [byerlikaya/AmazonWebServices](https://github.com/byerlikaya/AmazonWebServices)
- **NuGet Package**: [AmazonWebServices](https://www.nuget.org/packages/AmazonWebServices)
- **Sample Project**: [sample/AmazonWebServices.Sample.Api](../sample/AmazonWebServices.Sample.Api)
- **AWS S3 Documentation**: [Amazon S3 Developer Guide](https://docs.aws.amazon.com/s3/)
- **AWS SES Documentation**: [Amazon SES Developer Guide](https://docs.aws.amazon.com/ses/)

## 🆘 Destek ve Yardım

### Yaygın Problemler
Sık karşılaşılan sorunlar ve çözümleri için [Başlangıç Rehberi](./Getting-Started.md#7-yaygın-hatalar-ve-çözümleri) bölümünü inceleyin.

### Issue Reporting
Hata bildirimi için [GitHub Issues](https://github.com/byerlikaya/AmazonWebServices/issues) kullanın.

### Katkıda Bulunma
Projeye katkıda bulunmak için pull request gönderebilirsiniz.

## 📄 Lisans

Bu proje [LICENSE](../LICENSE) dosyasında belirtilen lisans ile korunmaktadır.

## 👨‍💻 Geliştirici

**Barış Yerlikaya**
- GitHub: [@byerlikaya](https://github.com/byerlikaya)
- E-posta: [GitHub Issues](https://github.com/byerlikaya/AmazonWebServices/issues) üzerinden iletişim kurabilirsiniz

---

**Not**: Bu dokümantasyon kütüphanenin v2.1.4.1 sürümü için hazırlanmıştır. Güncel bilgiler için [GitHub repository](https://github.com/byerlikaya/AmazonWebServices)'yi takip edin.