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
using System;
using System.IO;
using System.Threading.Tasks;

namespace AmazonWebServices.Services
{
    internal class AmazonS3Service : IAmazonS3Service
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

            var (folderName, fileName) = GetFolderAndFileName(uploadObjectRequest, addTimeStamp);
#if NETSTANDARD2_0
            using var newMemoryStream = new MemoryStream();
#endif
#if NETSTANDARD2_1
            await using var newMemoryStream = new MemoryStream();
#endif
            await uploadObjectRequest.File.CopyToAsync(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = GetKey(folderName, fileName),
                BucketName = _amazonS3Options.BucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            using var fileTransferUtility = new TransferUtility(_amazonS3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return GetUploadedFileName(uploadObjectRequest, folderName, fileName);
        }

        public async Task DeleteAsync(string fileName, string folderName = null) =>
            await _amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _amazonS3Options.BucketName,
                Key = GetKey(folderName, fileName)
            });

        private static (string folderName, string fileName) GetFolderAndFileName(UploadObjectRequest uploadObjectRequest, bool addTimeStamp)
        {
            var fileName = uploadObjectRequest.File.FileName;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var currentFileName = fileName.ToLowerInvariant().Replace(extension, string.Empty);

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

            var folderName = string.IsNullOrEmpty(uploadObjectRequest.FolderName)
                ? string.Empty
                : uploadObjectRequest.FolderName.Trim();

            return (folderName, fileName);
        }

        private static string GetKey(string folderName, string fileName) =>
            string.IsNullOrEmpty(folderName)
                ? fileName
                : $"{folderName}/{fileName}";

        private static string GetUploadedFileName(UploadObjectRequest uploadObjectRequest, string folderName, string fileName) =>
            string.IsNullOrEmpty(uploadObjectRequest.FolderName)
                ? fileName
                : $"{uploadObjectRequest.FolderName}/{folderName}";
    }
}
