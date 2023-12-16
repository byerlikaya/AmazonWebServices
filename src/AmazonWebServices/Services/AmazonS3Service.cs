namespace AmazonWebServices.Services;

internal class AmazonS3Service : IAmazonS3Service
{
    private readonly AmazonS3Client _amazonS3Client;
    private readonly AmazonS3Options _amazonS3Options;

    public AmazonS3Service(IConfiguration configuration)
    {
        var awsCredenatial = configuration
            .GetSection(nameof(AmazonCredentialOptions))
            .Get<AmazonCredentialOptions>();

        _amazonS3Options = configuration
            .GetSection(nameof(AmazonS3Options))
            .Get<AmazonS3Options>();

        if (_amazonS3Options is not null)
            _amazonS3Client = new AmazonS3Client(awsCredenatial.AccessKey, awsCredenatial.SecretKey, RegionEndpoint.GetBySystemName(_amazonS3Options.Region));
    }

    public async Task<string> UploadAsync(UploadObjectRequest uploadObjectRequest)
    {
        uploadObjectRequest.File.Verify();

        var (folderName, fileName) = GetFolderAndFileName(uploadObjectRequest);

        using var newMemoryStream = new MemoryStream();
        await uploadObjectRequest.File.CopyToAsync(newMemoryStream);

        var transferUtilityUploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = GetKey(folderName, fileName),
            BucketName = _amazonS3Options.BucketName,
            CannedACL = S3CannedACL.PublicRead
        };

        using var fileTransferUtility = new TransferUtility(_amazonS3Client);
        await fileTransferUtility.UploadAsync(transferUtilityUploadRequest);

        return GetUploadedFileName(uploadObjectRequest, fileName);
    }

    public async Task<string> UploadAsync(UploadRequest uploadRequest)
    {
        var (folderName, fileName) = GetFolderAndFileName(uploadRequest);

        var transferUtilityUploadRequest = new TransferUtilityUploadRequest
        {
            Key = GetKey(folderName, fileName),
            BucketName = _amazonS3Options.BucketName,
            CannedACL = S3CannedACL.PublicRead,
            InputStream = new FileStream(uploadRequest.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        };

        using var fileTransferUtility = new TransferUtility(_amazonS3Client);
        await fileTransferUtility.UploadAsync(transferUtilityUploadRequest);

        return GetUploadedFileName(uploadRequest, fileName);
    }

    public async Task<string> UploadExternalAwsAsync(UploadExternalAwsRequest uploadRequest)
    {
        var (folderName, fileName) = GetFolderAndFileName(uploadRequest);

        var transferUtilityUploadRequest = new TransferUtilityUploadRequest
        {
            Key = GetKey(folderName, fileName),
            BucketName = uploadRequest.AmazonS3Options.BucketName,
            CannedACL = S3CannedACL.PublicRead,
            InputStream = new FileStream(uploadRequest.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
        };

        var amazonS3Client = CreateAmazonS3Client(uploadRequest);

        using var fileTransferUtility = new TransferUtility(amazonS3Client);
        await fileTransferUtility.UploadAsync(transferUtilityUploadRequest);

        return GetUploadedFileName(uploadRequest, fileName);
    }

    private static AmazonS3Client CreateAmazonS3Client(UploadExternalAwsRequest uploadRequest) =>
        new(uploadRequest.AmazonCredentialOptions.AccessKey, uploadRequest.AmazonCredentialOptions.SecretKey, RegionEndpoint.GetBySystemName(uploadRequest.AmazonS3Options.Region));

    public async Task DeleteAsync(string fileName, string folderName = null) =>
        await _amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
        {
            BucketName = _amazonS3Options.BucketName,
            Key = GetKey(folderName, fileName)
        });

    private static (string folderName, string fileName) GetFolderAndFileName(UploadRequest uploadRequest)
    {
        var fileName = uploadRequest.FilePath;

        var folder = Path.GetDirectoryName(fileName);

        var file = fileName.Replace(folder!, string.Empty).Trim('\\');

        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        var currentFileName = file.ToLowerInvariant().Replace(extension, string.Empty);

        fileName = uploadRequest.AddTimeStamp
            ? GetFileNameWithTimeStamp(currentFileName, uploadRequest.FileName, extension)
            : GetFileName(currentFileName, uploadRequest.FileName, extension);

        var folderName = string.IsNullOrEmpty(uploadRequest.FolderName)
            ? string.Empty
            : uploadRequest.FolderName.Trim();

        return (folderName, fileName);
    }

    private static (string folderName, string fileName) GetFolderAndFileName(UploadObjectRequest uploadObjectRequest)
    {
        var fileName = uploadObjectRequest.File.FileName;

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var currentFileName = fileName.ToLowerInvariant().Replace(extension, string.Empty);

        fileName = uploadObjectRequest.AddTimeStamp
            ? GetFileNameWithTimeStamp(currentFileName, uploadObjectRequest.FileName, extension)
            : GetFileName(currentFileName, uploadObjectRequest.FileName, extension);

        var folderName = string.IsNullOrEmpty(uploadObjectRequest.FolderName)
            ? string.Empty
            : uploadObjectRequest.FolderName.Trim();

        return (folderName, fileName);
    }

    private static string GetFileName(
        string currentFileName,
        string fileName,
        string extension) =>
        string.IsNullOrEmpty(fileName)
            ? $"{currentFileName.ClearSpecialCharacters()}{extension}"
            : $"{fileName.ClearSpecialCharacters()}{extension}";

    private static string GetFileNameWithTimeStamp(
        string currentFileName,
        string fileName,
        string extension)
    {
        var timeStamp = DateTime.Now.ToString("yyMMdd-HHmmss-ffff");

        return string.IsNullOrEmpty(fileName)
            ? $"{currentFileName.ClearSpecialCharacters()}-{timeStamp}{extension}"
            : $"{fileName.ClearSpecialCharacters()}-{timeStamp}{extension}";
    }

    private static string GetKey(string folderName, string fileName) =>
        string.IsNullOrEmpty(folderName)
            ? fileName
            : $"{folderName}/{fileName}";

    private static string GetUploadedFileName(Requests.BaseUploadRequest uploadObjectRequest, string fileName) =>
        string.IsNullOrEmpty(uploadObjectRequest.FolderName)
            ? fileName
            : $"{uploadObjectRequest.FolderName}/{fileName}";
}