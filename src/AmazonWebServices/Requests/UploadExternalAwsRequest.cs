namespace AmazonWebServices.Requests;

public class UploadExternalAwsRequest : UploadRequest
{
    public AmazonCredentialOptions AmazonCredentialOptions { get; set; }

    public AmazonS3Options AmazonS3Options { get; set; }
}