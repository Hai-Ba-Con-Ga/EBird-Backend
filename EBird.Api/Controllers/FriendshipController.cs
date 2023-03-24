using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers
{
    [Route("friendship")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        //Create invitation 
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> CreateInvitation([FromBody] Guid receiverId)
        {
            Response<string> response = null;
            try
            {
                var userIdRaw = this.GetUserId();

                if(userIdRaw == null)
                    throw new UnauthorizedException("Not allow to access");

                Guid userId = Guid.Parse(userIdRaw);
                
                await _friendshipService.CreateInvitaion(userId, receiverId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int) HttpStatusCode.Created)
                    .SetMessage("Create invitation successfully")
                    .SetData("");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is UnauthorizedException)
                {
                    response = Response<Guid>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<Guid>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        //Accept invitation

        //Decline invitation    

        //Get lastest 10 invitation of a account

        //Get all friends of a account 

    }
}
