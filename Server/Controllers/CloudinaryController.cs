using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/cloudinary")]
    [ApiController]
    [AllowAnonymous]
    public class CloudinaryController(ICloudinaryRepo cloudinaryRepo) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var result = await cloudinaryRepo.UploadImageAsync(filePath);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> UploadImages(IFormFile[] files)
        {
            List<string> filePaths = new();

            foreach (var file in files)
            {
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                filePaths.Add(filePath);
            }

            var result = await cloudinaryRepo.UploadImagesAsync(filePaths.ToArray());

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
