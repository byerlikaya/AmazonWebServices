using AmazonWebServices.Requests;

namespace AmazonWebServices.Interfaces
{
    internal interface IFileService
    {
        Task<string> UploadAsync(UploadObjectRequest updaObjectRequest, bool addTimeStamp = false);

        Task DeleteAsync(string fileName, string folderName = null);
    }
}
