namespace AmazonWebServices.Interfaces;

public interface IAmazonS3Service
{
    Task<string> UploadAsync(UploadObjectRequest updaObjectRequest);

    Task<string> UploadAsync(UploadRequest uploadRequest);

    Task DeleteAsync(string fileName, string folderName = null);
}