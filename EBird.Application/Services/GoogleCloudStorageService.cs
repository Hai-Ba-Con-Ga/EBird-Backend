using EBird.Application.AppConfig;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using EBird.Application.Services.IServices;

namespace EBird.Application.Services
{
    public class GoogleCloudStorageService : ICloudStorage
    {
        private const string FIREBASE_STORAGE = "https://firebasestorage.googleapis.com/v0/b";


        private string buketName;
        private GoogleCredential googleCredential;
        private StorageClient storageClient;
        private readonly AppSetting appSetting;
        public GoogleCloudStorageService(IOptions<AppSetting> appSetting)
        {
            this.appSetting = appSetting.Value;
            var configuration = appSetting.Value;
            buketName = configuration.GoogleCloudStorageBucket;
            googleCredential = GoogleCredential.FromFile(configuration.FirebaseConfigPath);
            storageClient = StorageClient.Create(googleCredential);
        }
        public async Task DeleteFileAsync(string fileName)
        {
            await storageClient.DeleteObjectAsync(buketName, fileName);
        }
        private string ConvertCloudStorageUrlToFirebase(string fileName)
        {
            return $"{FIREBASE_STORAGE}/{appSetting.GoogleCloudStorageBucket}/o/{fileName}";
        }
        public async Task<string> UploadFileAsync(IFormFile imageFile, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                var uploadObject = await storageClient.UploadObjectAsync(buketName, fileName, imageFile.ContentType, memoryStream);
                return ConvertCloudStorageUrlToFirebase(fileName);
            }
        }
    }
}