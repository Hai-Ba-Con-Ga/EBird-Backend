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
        [HttpGet("all")]
        public async Task<ActionResult<ICollection<RequestResponse>>> Get([FromQuery] RequestParameters parameters)
        {
            Response<ICollection<RequestResponse>> response = null;
            try
            {
                ICollection<RequestResponse> listDTO = null;
                if (parameters.PageSize == 0)
                {
                    listDTO = await _requestService.GetRequests();
                }
                else
                {
                    listDTO = await _requestService.GetRequests(parameters);

                    var metaData = new
                    {
                        ((PagedList<RequestResponse>)listDTO).CurrentPage,
                        ((PagedList<RequestResponse>)listDTO).TotalPages,
                        ((PagedList<RequestResponse>)listDTO).PageSize,
                        ((PagedList<RequestResponse>)listDTO).HasNext,
                        ((PagedList<RequestResponse>)listDTO).HasPrevious,
                        ((PagedList<RequestResponse>)listDTO).TotalCount,
                    };

                    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
                }

                response = Response<ICollection<RequestResponse>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all request successful")
                    .SetData(listDTO);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<ICollection<RequestResponse>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<ICollection<RequestResponse>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }


        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<RequestResponse>>> Get(Guid id)
        {
            Response<RequestResponse> response = null;
            try
            {
                var requestDTO = await _requestService.GetRequest(id);

                response = Response<RequestResponse>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get request successful")
                    .SetData(requestDTO);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<RequestResponse>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RequestResponse>.Builder()
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

                entity.CreatedById = Guid.Parse(userId);

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
                if (ex is BadRequestException)
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
    }
}
