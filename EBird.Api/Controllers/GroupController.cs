using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Model;
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
        private IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            this._groupService = groupService;
            this._mapper = mapper;
        }

        // GET all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<GroupDTO>>>> Get()
        {
            Response<List<GroupDTO>> response = null;
            try
            {
                var responseData = await _groupService.GetGroups();

                response = Response<List<GroupDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.Created)
                    .SetMessage("Get groups is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<GroupDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }
                Console.WriteLine(ex.Message);

                response = Response<List<GroupDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<GroupDTO>>> Get(Guid id)
        {
            Response<GroupDTO> response = null;
            try
            {
                var responseData = await _groupService.GetGroup(id);

                response = Response<GroupDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.Created)
                    .SetMessage("Get group is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }
                Console.WriteLine(ex.Message);

                response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // POST create
        [HttpPost]
        public async Task<ActionResult<Response<GroupDTO>>> Post([FromBody] GroupDTO groupDTO)
        {
            Response<GroupDTO> response = null;
            try
            {
                var responseData = await _groupService.AddGroup(groupDTO);

                response = Response<GroupDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.Created)
                    .SetMessage("Create group is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }
                Console.WriteLine(ex.Message);

                response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // PUT update 
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<GroupDTO>>> Put(Guid id, [FromBody] GroupUpdateDTO groupUpdateDTO)
        {
            Response<GroupDTO> response = null;
            try
            {
                var responseData = await _groupService.UpdateGroup(id, groupUpdateDTO);

                response = Response<GroupDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Update group is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }
                Console.WriteLine(ex.Message);

                response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<GroupDTO>>> Delete(Guid id)
        {
            Response<GroupDTO> response = null;
            try
            {
                var responseData = await _groupService.DeleteGroup(id);

                response = Response<GroupDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.OK)
                    .SetMessage("Delete group is success")
                    .SetData(responseData);

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }
                Console.WriteLine(ex.Message);

                response = Response<GroupDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }
    }
}
