using Microsoft.AspNetCore.Http;
using MusicStore.Models;

namespace MusicStore.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            if (image.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("ImageUrl", "Plik nie może być większy niż 5MB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("ImageUrl", "Dozwolone są tylko pliki z rozszerzeniami .jpg, .jpeg, .png.");
            }

            string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, @"images/product");
            string uniqueFileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(imagesFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return @"/images/product/" + uniqueFileName;
        }
    }
}
