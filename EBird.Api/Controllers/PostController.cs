using EBird.Application.Exceptions;
using EBird.Application.Model.Post;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;
using System.Security.Claims;

namespace EBird.Api.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<Response<List<PostResponseDTO>>>> Get()
        {
            Response<List<PostResponseDTO>> response;
            try
            {
                var responseData = await _postService.GetPosts();
                response = new Response<List<PostResponseDTO>>()
                            .SetData(responseData)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get post is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<PostResponseDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<List<PostResponseDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<PostResponseDTO>>> Get(Guid id)
        {
            Response<PostResponseDTO> response;
            try
            {
                var responseData = await _postService.GetPost(id);
                response = new Response<PostResponseDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get post is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<PostResponseDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<PostResponseDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] PostRequestDTO postRequestDTO)
        {
            Response<Guid> response = null;
            string RawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (RawId == null)
            {
                response = Response<Guid>.Builder()
                    .SetSuccess(false)
                    .SetStatusCode((int)HttpStatusCode.BadRequest)
                    .SetMessage("Account not found");
            }
            try
            {
                Guid IdAccount = Guid.Parse(RawId);
                if (postRequestDTO.Thumbnail != null)
                {
                    postRequestDTO.Thumbnail.CreateById = IdAccount;
                }
                var postDTO = new PostRequestWithIdAccountDTO()
                {
                    Content = postRequestDTO.Content,
                    Title = postRequestDTO.Title,
                    CreateById = IdAccount,
                    Thumbnail = postRequestDTO.Thumbnail
                };
                Guid id = await _postService.AddPost(postDTO);

                response = new Response<Guid>()
                            .SetData(id)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Create post is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<Guid>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<Guid>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] PostRequestDTO ntDTO)
        {
            Response<string> response = null;
            try
            {
                await _postService.UpdatePost(id, ntDTO);
                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Update post is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                await _postService.DeletePost(id);
                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Delete post is successful");
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
