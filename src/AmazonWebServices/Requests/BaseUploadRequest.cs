namespace AmazonWebServices.Requests;

public class BaseUploadRequest
{
    public string FolderName { get; set; }
    public string FileName { get; set; }
    public bool AddTimeStamp { get; set; }
}