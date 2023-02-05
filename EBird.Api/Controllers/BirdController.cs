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
        public async Task<ActionResult<Response<IList<BirdDTO>>>> Get([FromQuery] BirdParameters parameters)
        {
            Response<IList<BirdDTO>> response = null;
            try
            {
                IList<BirdDTO> listBirdDTO = null;
                if (parameters.PageSize == 0)
                {
                    System.Console.WriteLine("params is null");
                    listBirdDTO = await _birdService.GetBirds();
                }
                else
                {
                    listBirdDTO = await _birdService.GetBirdsByPagingParameters(parameters);

                    var metaData = new
                    {
                        ((PagedList<BirdDTO>)listBirdDTO).CurrentPage,
                        ((PagedList<BirdDTO>)listBirdDTO).TotalPages,
                        ((PagedList<BirdDTO>)listBirdDTO).PageSize,
                        ((PagedList<BirdDTO>)listBirdDTO).HasNext,
                        ((PagedList<BirdDTO>)listBirdDTO).HasPrevious,
                        ((PagedList<BirdDTO>)listBirdDTO).TotalCount,
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
                }

                response = Response<IList<BirdDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all birds successful")
                    .SetData(listBirdDTO);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<IList<BirdDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<IList<BirdDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // GET : get bird by id 
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Get(Guid id)
        {
            Response<BirdDTO> response = null;
            try
            {
                var birdDTO = await _birdService.GetBird(id);

                response = Response<BirdDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get bird successful")
                    .SetData(birdDTO);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST : create bird
        [HttpPost]
        public async Task<ActionResult<Response<BirdDTO>>> Post([FromBody] BirdDTO birdDTO)
        {
            Response<BirdDTO> response = null;
            try
            {
                var responseData = await _birdService.AddBird(birdDTO);

                response = Response<BirdDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Create bird successful")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {

                if (ex is BadRequestException)
                {
                    response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // PUT : update bird
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Put(Guid id, [FromBody] BirdDTO birdDTO)
        {
            Response<BirdDTO> response = null;
            try
            {
                var responseData = await _birdService.UpdateBird(id, birdDTO);

                response = Response<BirdDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update bird successful")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException || ex is BadRequestException)
                {
                    response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<BirdDTO>.Builder()
                           .SetSuccess(false)
                           .SetStatusCode((int)HttpStatusCode.InternalServerError)
                           .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // DELETE : delete bird
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<BirdDTO>>> Delete(Guid id)
        {
            Response<BirdDTO> response = null;
            try
            {
                var responseData = await _birdService.DeleteBird(id);

                response = Response<BirdDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Delete bird successful")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException || ex is BadRequestException)
                {
                    response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                Console.WriteLine(ex.Message);

                response = Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }
        // GET: get all by account id
        [HttpGet("owner")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<List<BirdDTO>>>> GetAllByOwner()
        {
            Response<List<BirdDTO>> response = null;
            try
            {
                string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (rawId == null)
                {
                    throw new UnauthorizedException("Not allowed to access");
                }

                Guid accountId = Guid.Parse(rawId);

                Console.WriteLine(accountId);

                var responseData = await _birdService.GetAllBirdByAccount(accountId);

                response = Response<List<BirdDTO>>.Builder()
                   .SetSuccess(true)
                   .SetStatusCode((int)HttpStatusCode.OK)
                   .SetMessage("Get bird by account id is successful")
                   .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException || ex is UnauthorizedException)
                {
                    response = Response<List<BirdDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<List<BirdDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
