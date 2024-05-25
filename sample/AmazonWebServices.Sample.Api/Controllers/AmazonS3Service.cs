namespace AmazonWebServices.Sample.Api.Controllers;

[ApiController]
public class AmazonS3Service(IAmazonS3Service amazonS3Service) : ControllerBase
{
    [Consumes("multipart/form-data")]
    [Produces("multipart/form-data", "application/json")]
    [HttpPost("upload-object")]
    public async Task<IActionResult> Upload(IFormFile file, [FromQuery] string folderName, [FromQuery] string fileName)
    {
        var uploadedFileName = await amazonS3Service.UploadAsync(new UploadObjectRequest
        {
            File = file,
            FileName = fileName,
            FolderName = folderName
        });

        return Ok(uploadedFileName);
    }

    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromQuery] string filePath, [FromQuery] string folderName, [FromQuery] string fileName)
    {
        var uploadedFileName = await amazonS3Service.UploadAsync(new UploadRequest
        {
            FilePath = filePath,
            FileName = fileName,
            FolderName = folderName
        });

        return Ok(uploadedFileName);
    }

    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [HttpPost("upload-external")]
    public async Task<IActionResult> Upload(UploadExternalAwsRequest request)
    {
        var uploadedFileName = await amazonS3Service.UploadExternalAwsAsync(new UploadExternalAwsRequest
        {
            FilePath = request.FilePath,
            FileName = request.FileName,
            FolderName = request.FolderName,
            AmazonS3Options = request.AmazonS3Options,
            AmazonCredentialOptions = request.AmazonCredentialOptions
        });

        return Ok(uploadedFileName);
    }

    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromQuery] string folderName, [FromQuery] string fileName)
    {
        await amazonS3Service.DeleteAsync(fileName, folderName);
        return Ok();
    }
}