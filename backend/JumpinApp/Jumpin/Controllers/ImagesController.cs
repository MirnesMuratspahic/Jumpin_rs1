using Jumpin.Models;
using Jumpin.Models.DTO;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jumpin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService imageService;
        public ImagesController(IImageService _imageService)
        {
            this.imageService = _imageService;
        }

        // GET: {apibaseurl}/Images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            // Calling Image service to getting all images
            var images = await imageService.GetAllImages();

            // Converting Domain Model to DTO
            var response = new List<dtoVipImage>();
            foreach (var image in images)
            {
                response.Add(new dtoVipImage
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url
                });
            }
            return Ok(response);
        }

        // POST: {apibaseurl}/Images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                // Uploading file
                var vipImage = new VipImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now,
                };

                vipImage = await imageService.Upload(file, vipImage);

                // Converting Domain Model to DTO

                var response = new dtoVipImage
                {
                    Id = vipImage.Id,
                    Title = vipImage.Title,
                    DateCreated = vipImage.DateCreated,
                    FileExtension = vipImage.FileExtension,
                    FileName = vipImage.FileName,
                    Url = vipImage.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
