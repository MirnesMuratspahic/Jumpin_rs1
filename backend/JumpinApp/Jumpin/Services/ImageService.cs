using Jumpin.Context;
using Jumpin.Models;
using Jumpin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using static Jumpin.Services.Interfaces.IImageService;

namespace Jumpin.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext dbContext;
        public ImageService(IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor, ApplicationDbContext _dbContext)
        {
            this.webHostEnvironment = _webHostEnvironment;
            this.httpContextAccessor = _httpContextAccessor;
            this.dbContext = _dbContext;
        }

        public async Task<IEnumerable<VipImage>> GetAllImages()
        {
            return await dbContext.VipImages.ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        public async Task<VipImage> Upload(IFormFile file, VipImage blogImage)
        {
            //Uploading Image to API/ Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);

            await file.CopyToAsync(stream);

            // Updating the database
            var httpRequest = httpContextAccessor.HttpContext.Request;

            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await dbContext.VipImages.AddAsync(blogImage);
            await dbContext.SaveChangesAsync();

            return blogImage;
        }
    }
}
