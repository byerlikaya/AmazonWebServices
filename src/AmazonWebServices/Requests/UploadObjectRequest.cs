namespace AmazonWebServices.Requests;

public class UploadObjectRequest
{
    public IFormFile File { get; set; }
    public string FolderName { get; set; }
    public string FileName { get; set; }
}