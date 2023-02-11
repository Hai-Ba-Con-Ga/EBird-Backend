using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.BirdType;
using EBird.Application.Model.Place;
using EBird.Application.Services;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers
{
    [Route("place")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private IPlaceService _birdservice;

        public PlaceController(IPlaceService service)
        {
            _birdservice = service;
        }

        // GET all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<PlaceResponseDTO>>>> Get()
        {
            Response<List<PlaceResponseDTO>> response;
            try
            {
                var placeResponeList = await _birdservice.GetPlaces();

                response = new Response<List<PlaceResponseDTO>>()
                            .SetData(placeResponeList.ToList())
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get place is successful");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                {
                    response = Response<List<PlaceResponseDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<List<PlaceResponseDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }

        }

        // GET 
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<PlaceResponseDTO>>> Get(Guid id)
        {
            Response<PlaceResponseDTO> response;
            try
            {
                var placeRespone = await _birdservice.GetPlace(id);

                response = new Response<PlaceResponseDTO>()
                            .SetData(placeRespone)
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get place is successful");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {

                if (ex is BadRequestException)
                {
                    response = Response<PlaceResponseDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<PlaceResponseDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int)HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] PlaceRequestDTO placeDto)
        {
            Response<Guid> response = null;
            try
            {
                var createdId = await _birdservice.CreatePlace(placeDto);

                response = new Response<Guid>()
                            .SetData(createdId)
                            .SetStatusCode((int)HttpStatusCode.Created)
                            .SetSuccess(true)
                            .SetMessage("Create place is successful");

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
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // PUT update
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] PlaceRequestDTO placeDTO)
        {
            Response<string> response = null;
            try
            {
                await _birdservice.UpdatePlace(id, placeDTO);

                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Update place is successful");

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
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                await _birdservice.DeletepPlace(id);

                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int)HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Delete place is successful");

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
                        .SetMessage("Internal server error");

                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
