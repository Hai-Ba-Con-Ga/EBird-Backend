using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Model.Group;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;


namespace EBird.Api.Controllers
{
    [Route("/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            this._groupService = groupService;
        }

        // GET all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<GroupResponseDTO>>>> Get()
        {
            Response<List<GroupResponseDTO>> response = null;
            try
            {
                var responseData = await _groupService.GetGroups();

                response = Response<List<GroupResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Get groups is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<GroupResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<List<GroupResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GroupResponseDTO>>> Get(Guid id)
        {
            Response<GroupResponseDTO> response = null;
            try
            {
                var responseData = await _groupService.GetGroup(id);

                response = Response<GroupResponseDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Get group is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    response = Response<GroupResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<GroupResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<Response<string>>> Post([FromBody] GroupCreateDTO groupDTO)
        {
            Response<string> response = null;
            try
            {
                 await _groupService.AddGroup(groupDTO);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.Created)
                    .SetMessage("Create group is success")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.BadRequest)
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

        // PUT update 
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] GroupRequestDTO groupUpdateDTO)
        {
            Response<string> response = null;
            try
            {
                await _groupService.UpdateGroup(id, groupUpdateDTO);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Update group is success")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.BadRequest)
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

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                 await _groupService.DeleteGroup(id);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Delete group is success")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    response = Response<string>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.BadRequest)
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
    }
}
