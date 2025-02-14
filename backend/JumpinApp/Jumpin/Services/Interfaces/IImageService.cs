using Jumpin.Models;

namespace Jumpin.Services.Interfaces
{
    public interface IImageService
    {
            Task<VipImage> Upload(IFormFile file, VipImage blogImage);
            Task<IEnumerable<VipImage>> GetAllImages();
    }
}
