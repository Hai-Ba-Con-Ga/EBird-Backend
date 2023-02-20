using Microsoft.AspNetCore.Http;

namespace EBird.Application.Interfaces
{
    public interface ICloudStorage
    {
        Task DeleteFileAsync(string fileName);
        Task<string> UploadFileAsync(IFormFile imageFile, string fileName);
    }
}