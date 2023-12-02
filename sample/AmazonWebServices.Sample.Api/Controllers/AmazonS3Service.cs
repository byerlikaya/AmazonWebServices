namespace AmazonWebServices.Sample.Api.Controllers
{
    [ApiController]
    public class AmazonS3Service : ControllerBase
    {
        private readonly IAmazonS3Service _amazonS3Service;

        public AmazonS3Service(IAmazonS3Service amazonS3Service)
        {
            _amazonS3Service = amazonS3Service;
        }

        [Consumes("multipart/form-data")]
        [Produces("multipart/form-data", "application/json")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string folderName, [FromQuery] string fileName)
        {
            var uploadedFileName = await _amazonS3Service.UploadAsync(new UploadObjectRequest
            {
                File = file,
                FileName = fileName,
                FolderName = folderName
            });

            return Ok(uploadedFileName);
        }

        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromQuery] string folderName, [FromQuery] string fileName)
        {
            await _amazonS3Service.DeleteAsync(fileName, folderName);
            return Ok();
        }
    }
}
