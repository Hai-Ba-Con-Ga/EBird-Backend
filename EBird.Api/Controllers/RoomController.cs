using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

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


        [HttpGet("all")]
        public async Task<ActionResult<Response<List<RoomResponseDTO>>>> GetRooms()
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


        [HttpGet("{id}")]
        public async Task<ActionResult<Response<RoomResponseDTO>>> GetRoom(Guid id)
        {
            Response<RoomResponseDTO> response = null;
            try
            {
                var responseData = await _roomService.GetRoom(id);

                response = Response<RoomResponseDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
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



        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> CreateRoom([FromBody] RoomCreateDTO RoomCreateDTO)
        {
            var response = new Response<string>();
            string rawId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            response ??= Response<string>.Builder().SetSuccess(false)
                .SetStatusCode((int)HttpStatusCode.NotFound).SetMessage("Not Allow to access");

            try
            {
                Guid id = Guid.Parse(rawId);
                await _roomService.AddRoom(id, RoomCreateDTO);
                response = Response<string>.Builder().SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.Created).SetMessage("Create room is success").SetData("");
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder().SetSuccess(false)
                        .SetStatusCode(((BaseHttpException)ex).StatusCode).SetMessage(ex.Message);
                    return StatusCode((int)response.StatusCode, response);
                }
                response = Response<string>.Builder().SetSuccess(false)
                    .SetStatusCode((int)HttpStatusCode.InternalServerError).SetMessage("Internal Server Error");
                return StatusCode((int)response.StatusCode, response);
            }
        }


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
