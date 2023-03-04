using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
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

                listBirdDTO = await _birdService.GetBirdsByPagingParameters(parameters);

                PagingData metaData = new PagingData()
                {
                    CurrentPage = ((PagedList<BirdResponseDTO>)listBirdDTO).CurrentPage,
                    PageSize = ((PagedList<BirdResponseDTO>)listBirdDTO).PageSize,
                    TotalCount = ((PagedList<BirdResponseDTO>)listBirdDTO).TotalCount,
                    TotalPages = ((PagedList<BirdResponseDTO>)listBirdDTO).TotalPages,
                    HasNext = ((PagedList<BirdResponseDTO>)listBirdDTO).HasNext,
                    HasPrevious = ((PagedList<BirdResponseDTO>)listBirdDTO).HasPrevious
                };

                response = ResponseWithPaging<IList<BirdResponseDTO>>.Builder()
                .SetSuccess(true)
                .SetStatusCode((int)HttpStatusCode.OK)
                .SetMessage("Get all birds successful")
                .SetData(listBirdDTO)
                .SetPagingData(metaData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<IList<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<IList<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
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
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get bird successful")
                    .SetData(birdDTO);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<BirdResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST : create bird
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] BirdCreateDTO birdDTO)
        {
            Response<Guid> response = null;
            try
            {
                string userId = this.GetUserId();

                if (userId == null) throw new UnauthorizedException("Not allow to access");

                birdDTO.OwnerId = Guid.Parse(userId);

                if (birdDTO.ListResource != null)
                {
                    foreach (var resource in birdDTO.ListResource)
                    {
                        resource.CreateById = Guid.Parse(userId);
                    }
                }

                var birdId = await _birdService.AddBird(birdDTO);

                response = Response<Guid>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Create bird successful")
                    .SetData(birdId);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
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
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // PUT : update bird
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] BirdRequestDTO birdDTO)
        {
            Response<string> response = null;
            try
            {
                var userIdRaw = this.GetUserId();

                if (userIdRaw == null)
                    throw new UnauthorizedException("Not allow to access");

                var userId = Guid.Parse(userIdRaw);

                await _birdService.UpdateBird(id, birdDTO, userId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update bird successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
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
                           .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // DELETE : delete bird
        [HttpDelete("{birdId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> Delete(Guid birdId)
        {
            Response<string> response = null;
            try
            {
                var userIdRaw = this.GetUserId();

                if (userIdRaw == null)
                    throw new UnauthorizedException("Not allow to access");

                Guid userId = Guid.Parse(userIdRaw);

                await _birdService.DeleteBird(userId, birdId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Delete bird successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is NotFoundException || ex is BadRequestException)
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
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
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
                string rawId = this.GetUserId();
                if (rawId == null)
                {
                    throw new UnauthorizedException("Not allowed to access");
                }

                Guid accountId = Guid.Parse(rawId);

                var responseData = await _birdService.GetAllBirdByAccount(accountId);

                response = Response<List<BirdResponseDTO>>.Builder()
                   .SetSuccess(true)
                   .SetStatusCode((int)HttpStatusCode.OK)
                   .SetMessage("Get bird by account id is successful")
                   .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is UnauthorizedException ||
                    ex is NotFoundException)
                {
                    response = Response<List<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<List<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // GET: get all by account id sort by Elo desc
        [HttpGet("owner/{id}")]
        public async Task<ActionResult<Response<List<BirdResponseDTO>>>> GetAllByOwnerSortByEloDesc(Guid id)
        {
            Response<List<BirdResponseDTO>> response = null;
            try
            {
                var responseData = await _birdService.GetAllBirdByAccount(id);
                responseData = responseData.OrderBy(x => -x.Elo).ToList();

                response = Response<List<BirdResponseDTO>>.Builder()
                   .SetSuccess(true)
                   .SetStatusCode((int)HttpStatusCode.OK)
                   .SetMessage("Get bird by account id is successful")
                   .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is UnauthorizedException ||
                    ex is NotFoundException)
                {
                    response = Response<List<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<List<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

    }
}
