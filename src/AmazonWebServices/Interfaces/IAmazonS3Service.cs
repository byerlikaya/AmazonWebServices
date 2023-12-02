namespace AmazonWebServices.Interfaces;

public interface IAmazonS3Service
{
    Task<string> UploadAsync(UploadObjectRequest updaObjectRequest, bool addTimeStamp = false);

    Task DeleteAsync(string fileName, string folderName = null);
}