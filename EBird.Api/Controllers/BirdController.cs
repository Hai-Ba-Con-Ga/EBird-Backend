using EBird.Application.Exceptions;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Response;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers
{
    [Route("bird")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        private IBirdService _birdService;

        public BirdController(IBirdService birdService)
        {
            this._birdService = birdService;
        }


        // GET: get all
        [HttpGet("all")]
        public async Task<ActionResult<Response<IList<BirdResponseDTO>>>> Get([FromQuery] BirdParameters parameters)
        {
            Response<IList<BirdResponseDTO>> response = null;
            try
            {
                IList<BirdResponseDTO> listBirdDTO = null;
                if(parameters.PageSize == 0)
                {
                    System.Console.WriteLine("params is null");
                    listBirdDTO = await _birdService.GetBirds();
                }
                else
                {
                    listBirdDTO = await _birdService.GetBirdsByPagingParameters(parameters);

                    var metaData = new
                    {
                        ((PagedList<BirdResponseDTO>) listBirdDTO).CurrentPage,
                        ((PagedList<BirdResponseDTO>) listBirdDTO).TotalPages,
                        ((PagedList<BirdResponseDTO>) listBirdDTO).PageSize,
                        ((PagedList<BirdResponseDTO>) listBirdDTO).HasNext,
                        ((PagedList<BirdResponseDTO>) listBirdDTO).HasPrevious,
                        ((PagedList<BirdResponseDTO>) listBirdDTO).TotalCount,
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
                }

                response = Response<IList<BirdResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Get all birds successful")
                    .SetData(listBirdDTO);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<IList<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }
                response = Response<IList<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // GET : get bird by id 
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdResponseDTO>>> Get(Guid id)
        {
            Response<BirdResponseDTO> response = null;
            try
            {
                var birdDTO = await _birdService.GetBird(id);

                response = Response<BirdResponseDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Get bird successful")
                    .SetData(birdDTO);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<BirdResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // POST : create bird
        [HttpPost]
        public async Task<ActionResult<Response<string>>> Post([FromBody] BirdCreateDTO birdDTO)
        {
            Response<string> response = null;
            try
            {
                await _birdService.AddBird(birdDTO);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.Created)
                    .SetMessage("Create bird successful")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {

                if(ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // PUT : update bird
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] BirdRequestDTO birdDTO)
        {
            Response<string> response = null;
            try
            {
                await _birdService.UpdateBird(id, birdDTO);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Update bird successful")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is NotFoundException || ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<string>.Builder()
                           .SetSuccess(false)
                           .SetStatusCode((int) HttpStatusCode.InternalServerError)
                           .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // DELETE : delete bird
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                await _birdService.DeleteBird(id);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Delete bird successful")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is NotFoundException || ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }
        
        // GET: get all by account id
        [HttpGet("owner")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<List<BirdResponseDTO>>>> GetAllByOwner()
        {
            Response<List<BirdResponseDTO>> response = null;
            try
            {
                string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(rawId == null)
                {
                    throw new UnauthorizedException("Not allowed to access");
                }

                Guid accountId = Guid.Parse(rawId);

                Console.WriteLine(accountId);

                var responseData = await _birdService.GetAllBirdByAccount(accountId);

                response = Response<List<BirdResponseDTO>>.Builder()
                   .SetSuccess(true)
                   .SetStatusCode((int) HttpStatusCode.OK)
                   .SetMessage("Get bird by account id is successful")
                   .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException || ex is UnauthorizedException)
                {
                    response = Response<List<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<List<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }
    }
}
