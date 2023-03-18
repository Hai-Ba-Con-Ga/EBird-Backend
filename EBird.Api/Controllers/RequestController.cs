using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Request;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Response;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers
{
    [Route("request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private IRequestService _requestService;

        public RequestController(IRequestService service)
        {
            this._requestService = service;
        }

        // GET: all
        [HttpGet("room")]
        public async Task<ActionResult<Response<ICollection<RequestResponseDTO>>>> GetRequestFromAllRoom([FromQuery] RequestParameters parameters)
        {
            // ResponseWithPaging<ICollection<RequestResponse>> response = null;
            Response<ICollection<RequestResponseDTO>> response;
            try
            {
                ICollection<RequestResponseDTO> listDTO = null;

                if (parameters.PageSize == 0 && parameters.RoomId == null)
                {
                    listDTO = await _requestService.GetRequests();

                    response = Response<ICollection<RequestResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all request successful")
                    .SetData(listDTO);
                }
                else
                {
                    listDTO = await _requestService.GetRequests(parameters);

                    PagingData metaData = new PagingData()
                    {
                        CurrentPage = ((PagedList<RequestResponseDTO>)listDTO).CurrentPage,
                        PageSize = ((PagedList<RequestResponseDTO>)listDTO).PageSize,
                        TotalCount = ((PagedList<RequestResponseDTO>)listDTO).TotalCount,
                        TotalPages = ((PagedList<RequestResponseDTO>)listDTO).TotalPages,
                        HasNext = ((PagedList<RequestResponseDTO>)listDTO).HasNext,
                        HasPrevious = ((PagedList<RequestResponseDTO>)listDTO).HasPrevious
                    };   

                    response = ResponseWithPaging<ICollection<RequestResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all request successful")
                    .SetData(listDTO)
                    .SetPagingData(metaData);
                }

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<ICollection<RequestResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                Console.WriteLine($"Error: {ex.Message}");
                
                response = Response<ICollection<RequestResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<RequestResponseDTO>>> Get(Guid id)
        {
            Response<RequestResponseDTO> response = null;
            try
            {
                var RequestResponse = await _requestService.GetRequest(id);

                response = Response<RequestResponseDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get request successful")
                    .SetData(RequestResponse);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<RequestResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RequestResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<Response<ICollection<RequestResponseDTO>>>> GetRequestsByGroupId(Guid groupId, [FromQuery] RequestParameters parameters)
        {
            Response<ICollection<RequestResponseDTO>> response;
            try
            {
                ICollection<RequestResponseDTO> listDTO = null;

                listDTO = await _requestService.GetRequestsByGroupId(groupId, parameters);

                PagingData metaData = new PagingData()
                {
                    CurrentPage = ((PagedList<RequestResponseDTO>)listDTO).CurrentPage,
                    PageSize = ((PagedList<RequestResponseDTO>)listDTO).PageSize,
                    TotalCount = ((PagedList<RequestResponseDTO>)listDTO).TotalCount,
                    TotalPages = ((PagedList<RequestResponseDTO>)listDTO).TotalPages,
                    HasNext = ((PagedList<RequestResponseDTO>)listDTO).HasNext,
                    HasPrevious = ((PagedList<RequestResponseDTO>)listDTO).HasPrevious
                };

                response = ResponseWithPaging<ICollection<RequestResponseDTO>>.Builder()
                .SetSuccess(true)
                .SetStatusCode((int)HttpStatusCode.OK)
                .SetMessage($"Get all request in {groupId} successful")
                .SetData(listDTO)
                .SetPagingData(metaData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<ICollection<RequestResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<ICollection<RequestResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST api/<RequestController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] RequestCreateDTO entity)
        {
            Response<Guid> response = null;
            try
            {
                var userId = this.GetUserId();

                if (userId == null)
                    throw new UnauthorizedException("Not allow to acces this resource");

                entity.HostId = Guid.Parse(userId);

                var createdId = await _requestService.CreateRequest(entity);

                response = Response<Guid>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Create request successful")
                    .SetData(createdId);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<Guid>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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

        // PUT update
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] RequestUpdateDTO entity)
        {
            Response<string> response = null;
            try
            {
                await _requestService.UpdateRequest(id, entity);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update request successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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

        // DELETE api/<RequestController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                await _requestService.DeleteRequest(id);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Delete request successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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

        [HttpPut("join/{requestId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> JoinRequest(Guid requestId, [FromBody] JoinRequestDTO joinRequestResponse)
        {
            Response<string> response = null;
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null)
                    throw new UnauthorizedException("Not allow to access this resource");

                Guid userId = Guid.Parse(userRawId);

                await _requestService.JoinRequest(requestId, userId, joinRequestResponse);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Join request successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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

        [HttpPost("merge")]
        public async Task<ActionResult<Response<string>>> Merge([FromBody] RequestMergeDTO requestMergeDto)
        {
            Response<string> response = null;
            try
            {
                var newRequestId = await _requestService.MergeRequest(requestMergeDto.hostRequestId, requestMergeDto.challengerRequestId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Join request successful")
                    .SetData(newRequestId.ToString());

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                Console.WriteLine($"Error: {ex.Message}");

                response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        //Put: change request IsReady to true
        [HttpPut("ready/{requestId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> ReadyRequest(Guid requestId)
        {
            Response<string> response = null;
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null)
                    throw new UnauthorizedException("Not allow to access this resource");

                Guid userId = Guid.Parse(userRawId);

                await _requestService.ReadyRequest(requestId, userId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Ready request successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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

        //Get: get all request where hostId = userId or challengerId = userId
        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<ICollection<RequestResponseDTO>>>> GetRequestByUserId()
        {
            Response<ICollection<RequestResponseDTO>> response = null;
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null)
                    throw new UnauthorizedException("Not allow to access this resource");

                Guid userId = Guid.Parse(userRawId);

                var requests = await _requestService.GetRequestByUserId(userId);

                response = Response<ICollection<RequestResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get request successful")
                    .SetData(requests);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<ICollection<RequestResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<ICollection<RequestResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        //Post: check tow request come from same user
        [HttpPut("check")]
        public async Task<ActionResult<Response<bool>>> CheckRequest([FromBody] RequestCheckDTO requestCheckDto)
        {
            Response<bool> response = null;
            try
            {
                bool isValid = await _requestService.CheckRequest(requestCheckDto.HostRequestID, requestCheckDto.ChallengerRequestID);

                string message = "";

                if (isValid == false)
                {
                    message = "Tow requests are not valid to merge";
                }
                else
                {
                    message = "Tow requests are valid to merge";
                }

                response = Response<bool>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage(message)
                    .SetData(isValid);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<bool>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                Console.WriteLine($"Error: {ex.Message}");

                response = Response<bool>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // Put: leave off 
        [HttpPut("leave/{requestId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> LeaveRequest(Guid requestId)
        {
            Response<string> response = null;
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null)
                    throw new UnauthorizedException("Not allow to access this resource");

                Guid userId = Guid.Parse(userRawId);

                await _requestService.LeaveRequest(requestId, userId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Leave out request successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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


        //Put: change request IsReady to true// Put: leave off 
        [HttpPut("kick/{requestId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> KickRequest(Guid requestId, [FromBody] Guid kickedUserId)
        {
            Response<string> response = null;
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null)
                    throw new UnauthorizedException("Not allow to access this resource");

                Guid userId = Guid.Parse(userRawId);

                await _requestService.KickFromRequest(requestId, userId, kickedUserId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Kick challenger from request successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
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

    }
}
