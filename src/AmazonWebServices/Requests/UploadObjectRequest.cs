namespace AmazonWebServices.Requests;

public class UploadObjectRequest : BaseUploadRequest
{
    public IFormFile File { get; set; }
}