using EBird.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EBird.Application.Services.IServices
{
    public interface IFileServices
    {
        Task<ResourceEntity> UploadFileAsync(IFormFile imageFile, string fileName, Guid CreateId);
        Task DeleteFileAsync(string fileName);
    }
}