
using System.Net;
using System.Security.Claims;
using EBird.Api.UserFeatures.Requests;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileServices _fileServices;
        public FileController(IFileServices fileServices)
        {
            _fileServices = fileServices;
        }
        [HttpPost("upload-image")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResourceEntity>> UploadFile([FromForm] UploadRequest request)
        {
            var response = new Response<ResourceEntity>();
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (rawId == null)
            {
                response = Response<ResourceEntity>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.NotFound).SetMessage("Account not found");
            }
            try
            {
                Guid createId = Guid.Parse(rawId);
                var resource = await _fileServices.UploadFileAsync(request.FileUpload, request.FileUpload.FileName, createId);
                response = Response<ResourceEntity>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Upload file success").SetData(resource);
            }
            catch (Exception ex)
            {
                response = Response<ResourceEntity>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpDelete("{file-name}")]
        public async Task<ActionResult<Response<string>>> DeleteFile(string fileName)
        {
            var response = new Response<string>();
            try
            {
                await _fileServices.DeleteFileAsync(fileName);
                response = Response<string>.Builder().SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK).SetMessage("Delete file success");
            }
            catch (Exception ex)
            {
                response = Response<string>.Builder().SetSuccess(false).SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage(ex.Message);
            }
            return StatusCode((int)response.StatusCode, response);
        }
    }
}