using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EBird.Api.Controllers
{
    [Route("/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomService _roomService;
        private IMapper _mapper;

        public RoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        // get all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<RoomResponseDTO>>>> Get()
        {
            Response<List<RoomResponseDTO>> response = null;
            try
            {
                var responseData = await _roomService.GetRooms();

                response = Response<List<RoomResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Get room list is success")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<RoomResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<List<RoomResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }


        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<RoomResponseDTO>>> Get(Guid id)
        {
            Response<RoomResponseDTO> response = null;
            try
            {
                var responseData = await _roomService.GetRoom(id);

                response = Response<RoomResponseDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Get room is success")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<RoomResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RoomResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<Response<string>>> Post([FromBody] RoomCreateDTO RoomCreateDTO)
        {
            Response<string> response = null;
            try
            {
                await _roomService.AddRoom(RoomCreateDTO);
                
                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Create room is success")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
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
        // PUT update 
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] RoomUpdateDTO roomUpdateDTO)
        {
            Response<string> response = null;
            try
            {
                await _roomService.UpdateRoom(id, roomUpdateDTO);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update room is success")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
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

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                await _roomService.DeleteRoom(id);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Delete room is success")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
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
