using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.GroupMember;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{
    [ApiController]
    [Route("groupmember")]
    public class GroupMemberController : ControllerBase
    {
        private readonly IGroupMemberService _groupMemberService;

        public GroupMemberController(IGroupMemberService groupMemberService)
        {
            _groupMemberService = groupMemberService;
        }

        //join group
        [HttpPost("join/{groupId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<Guid>>> JoinGroup(Guid groupId)
        {
            Response<Guid> response = null;
            try
            {
                string rawUserId = this.GetUserId();

                Guid userId = Guid.Parse(rawUserId);

                if (userId == Guid.Empty)
                    throw new UnauthorizedException("Not allow to access");

                Guid createdGroupMemberId = await _groupMemberService.JoinGroup(userId, groupId);

                response = Response<Guid>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Join group successful")
                    .SetData(createdGroupMemberId);

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

        //out group
        [HttpPut("out/{groupId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> OutGroup(Guid groupId)
        {
            Response<string> response = null;
            try
            {
                string rawUserId = this.GetUserId();

                Guid userId = Guid.Parse(rawUserId);

                if (userId == Guid.Empty)
                    throw new UnauthorizedException("Not allow to access");

                await _groupMemberService.OutGroup(userId, groupId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Out group successful")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
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

         //get all group user have joined
        [HttpGet("in")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<ICollection<GroupMemberResponseDTO>>>> GetGroupHaveJoined()
        {
            Response<ICollection<GroupMemberResponseDTO>> response = null;
            try
            {
                string rawUserId = this.GetUserId();

                Guid userId = Guid.Parse(rawUserId);

                if (userId == Guid.Empty)
                    throw new UnauthorizedException("Not allow to access");

                var groupMemberList = await _groupMemberService.GetGroupsHaveJoined(userId);

                response = Response<ICollection<GroupMemberResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get group have joined successful")
                    .SetData(groupMemberList);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<ICollection<GroupMemberResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                Console.WriteLine($"Error: {ex.Message}");
                
                response = Response<ICollection<GroupMemberResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }
    }
}