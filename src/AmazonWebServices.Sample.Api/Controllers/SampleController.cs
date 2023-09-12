using AmazonWebServices.Interfaces;
using AmazonWebServices.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AmazonWebServices.Sample.Api.Controllers
{
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IAmazonS3Service _amazonS3Service;

        public SampleController(IAmazonS3Service amazonS3Service)
        {
            _amazonS3Service = amazonS3Service;
        }


        [Consumes("multipart/form-data")]
        [Produces("multipart/form-data", "application/json")]
        [HttpPost("/file/upload")]
        public async Task<IActionResult> FileUpload([FromForm] IFormFile file, [FromQuery] string folderName, [FromQuery] string fileName)
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
        [HttpPost("/file/delete")]
        public async Task<IActionResult> FileDelete([FromQuery] string folderName, [FromQuery] string fileName)
        {
            await _amazonS3Service.DeleteAsync(fileName, folderName);
            return Ok();
        }
    }
}