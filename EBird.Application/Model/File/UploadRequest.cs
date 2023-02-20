using Microsoft.AspNetCore.Http;

namespace EBird.Application.Model.File{
    public class UploadRequest{
        public IFormFile FileUpload { get; set; }
    }
}