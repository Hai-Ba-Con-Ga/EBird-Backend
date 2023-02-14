using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Model.Match;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{
    [ApiController]
    [Route("match-bird")]
    public class MatchBirdController : ControllerBase
    {
        private readonly IMatchBirdService _matchBirdService;

        public MatchBirdController(IMatchBirdService matchBirdService)
        {
            _matchBirdService = matchBirdService;
        }

        //update bird in match
        [HttpPut]
        public async Task<ActionResult<Response<string>>> Put([FromBody] MatchBirdUpdateDTO updateData)
        {
            var response = new Response<string>();
            try
            {
                await _matchBirdService.UpdateBirdInMatch(updateData);
                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update bird in match is success")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
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
    }
}