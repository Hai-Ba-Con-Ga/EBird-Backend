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
        public async Task<ActionResult<Response<List<RoomDTO>>>> Get()
        {
            Response<List<RoomDTO>> response = null;
            try
            {
                var responseData = await _roomService.GetRooms();

                response = Response<List<RoomDTO>>.Builder()
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
                    response = Response<List<RoomDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<List<RoomDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }


        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<RoomDTO>>> Get(Guid id)
        {
            Response<RoomDTO> response = null;
            try
            {
                var responseData = await _roomService.GetRoom(id);

                response = Response<RoomDTO>.Builder()
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
                    response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<Response<RoomDTO>>> Post([FromBody] RoomDTO RoomDTO)
        {
            Response<RoomDTO> response = null;
            try
            {
                var responseData = await _roomService.AddRoom(RoomDTO);
                
                response = Response<RoomDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created)
                    .SetMessage("Create room is success")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }
        // PUT update 
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<RoomDTO>>> Put(Guid id, [FromBody] RoomDTO roomDTO)
        {
            Response<RoomDTO> response = null;
            try
            {
                var responseData = await _roomService.UpdateRoom(id, roomDTO);

                response = Response<RoomDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update room is success")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<RoomDTO>>> Delete(Guid id)
        {
            Response<RoomDTO> response = null;
            try
            {
                var responseData = await _roomService.DeleteRoom(id);

                response = Response<RoomDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Delete room is success")
                    .SetData(responseData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException)ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<RoomDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}
