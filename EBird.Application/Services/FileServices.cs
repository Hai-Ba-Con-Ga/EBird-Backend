using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Http;
using EBird.Application.Services.IServices;

namespace EBird.Application.Services
{
    public class FileServices : IFileServices
    {
        private readonly ICloudStorage _cloudStorage;
        private readonly IGenericRepository<ResourceEntity> _resourceRepository;
        public FileServices(ICloudStorage cloudStorage, IGenericRepository<ResourceEntity> resourceRepository)
        {
            _cloudStorage = cloudStorage;
            _resourceRepository = resourceRepository;
        }
        public async Task<ResourceEntity> UploadFileAsync(IFormFile imageFile, string fileName, Guid CreateId)
        {
            var path =  await _cloudStorage.UploadFileAsync(imageFile, fileName);
            var resource = new ResourceEntity(){
                Datalink = path,
                CreateById = CreateId
            };
            await _resourceRepository.CreateAsync(resource);
            return resource;
        }
        public async Task DeleteFileAsync(string fileName)
        {
            await _cloudStorage.DeleteFileAsync(fileName);
        }
    }
}