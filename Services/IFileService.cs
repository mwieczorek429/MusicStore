
namespace MusicStore.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile image);
    }
}