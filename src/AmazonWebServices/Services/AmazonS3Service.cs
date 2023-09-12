using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AmazonWebServices.Extensions;
using AmazonWebServices.Interfaces;
using AmazonWebServices.Options;
using AmazonWebServices.Requests;
using AmazonWebServices.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AmazonWebServices.Services
{
    internal class AmazonS3Service : IFileService
    {
        private readonly AmazonS3Client _amazonS3Client;
        private readonly AmazonS3Options _amazonS3Options;

        public AmazonS3Service()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();

            var awsCredenatial = configuration
                .GetSection(nameof(AmazonCredentialOptions))
                .Get<AmazonCredentialOptions>();

            _amazonS3Client = new AmazonS3Client(awsCredenatial.AccessKey, awsCredenatial.SecretKey, RegionEndpoint.EUCentral1);

            _amazonS3Options = configuration
                .GetSection(nameof(AmazonS3Options))
                .Get<AmazonS3Options>();
        }

        public async Task<string> UploadAsync(UploadObjectRequest uploadObjectRequest, bool addTimeStamp = false)
        {
            uploadObjectRequest.File.Verify();

            var fileName = uploadObjectRequest.File.FileName;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var currentFileName = fileName.Replace(extension, string.Empty);

            if (addTimeStamp)
            {
                var timeStamp = DateTime.Now.ToString("yyMMdd-HHmmss-ffff");

                fileName = string.IsNullOrEmpty(uploadObjectRequest.FileName)
                    ? $"{currentFileName.ClearSpecialCharacters()}-{timeStamp}{extension}"
                    : $"{uploadObjectRequest.FileName.ClearSpecialCharacters()}-{timeStamp}{extension}";
            }
            else
            {
                fileName = string.IsNullOrEmpty(uploadObjectRequest.FileName)
                    ? $"{currentFileName.ClearSpecialCharacters()}{extension}"
                    : $"{uploadObjectRequest.FileName.ClearSpecialCharacters()}{extension}";
            }

            await using var newMemoryStream = new MemoryStream();
            await uploadObjectRequest.File.CopyToAsync(newMemoryStream);

            var folderName = string.IsNullOrEmpty(uploadObjectRequest.FolderName)
                ? string.Empty
                : uploadObjectRequest.FolderName.Trim();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = string.IsNullOrEmpty(folderName)
                    ? fileName
                    : $"{folderName}/{fileName}",
                BucketName = _amazonS3Options.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            using var fileTransferUtility = new TransferUtility(_amazonS3Client);

            await fileTransferUtility.UploadAsync(uploadRequest);

            return string.IsNullOrEmpty(uploadObjectRequest.FolderName)
                ? fileName
                : $"{uploadObjectRequest.FolderName}/{fileName}";
        }

        public async Task DeleteAsync(string fileName, string folderName = null)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _amazonS3Options.BucketName,
                Key = string.IsNullOrEmpty(folderName)
                    ? fileName
                    : $"{folderName}/{fileName}"
            };

            await _amazonS3Client.DeleteObjectAsync(deleteRequest);
        }
    }
}
