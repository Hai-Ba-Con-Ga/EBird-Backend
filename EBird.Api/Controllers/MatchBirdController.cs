using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.Match;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut("result")]
        public async Task<ActionResult<Response<string>>> UpdateResult([FromBody] UpdateMatchResultDTO updateData)
        {
            var response = new Response<string>();
            try
            {
                await _matchBirdService.UpdateMatchResult(updateData);
                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update match result is success")
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

        [HttpPut("challenger/ready")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> UpdateHostReady([FromBody] UpdateChallengerToReadyDTO updateData)
        {
            var response = new Response<string>();
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null) throw new UnauthorizedException("Not allow to access");

                updateData.ChallengerId = Guid.Parse(userRawId);

                await _matchBirdService.UpdateChallengerReady(updateData);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update host ready is success")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is NotFoundException ||
                    ex is UnauthorizedException)
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

        [HttpPut("result/{matchId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<string>>> UpdateResult(Guid matchId, [FromBody] MatchBirdUpdateResultDTO updateResultDto)
        {
            var response = new Response<string>();
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null) throw new UnauthorizedException("Not allow to access");

                Guid userId = Guid.Parse(userRawId);

                await _matchBirdService.UpdateResultMatch(matchId, updateResultDto, userId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update result is success")
                    .SetData("");

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || 
                    ex is NotFoundException ||
                    ex is UnauthorizedException)
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