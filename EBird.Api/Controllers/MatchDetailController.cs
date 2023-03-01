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
    [Route("match-detail")]
    public class MatchDetailController : ControllerBase
    {
        private readonly IMatchDetailService _matchDetailService;

        public MatchDetailController(IMatchDetailService matchBirdService)
        {
            _matchDetailService = matchBirdService;
        }

        //update bird in match
        [HttpPut]
        public async Task<ActionResult<Response<string>>> Put([FromBody] MatchDetailUpdateDTO updateData)
        {
            var response = new Response<string>();
            try
            {
                await _matchDetailService.UpdateBirdInMatch(updateData);
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

        // [HttpPut("result")]
        // public async Task<ActionResult<Response<string>>> UpdateResult([FromBody] UpdateMatchResultDTO updateData)
        // {
        //     var response = new Response<string>();
        //     try
        //     {
        //         await _matchDetailService.UpdateMatchResult(updateData);
        //         response = Response<string>.Builder()
        //             .SetSuccess(true)
        //             .SetStatusCode((int)HttpStatusCode.OK)
        //             .SetMessage("Update match result is success")
        //             .SetData("");

        //         return StatusCode((int)response.StatusCode, response);
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is NotFoundException)
        //         {
        //             response = Response<string>.Builder()
        //                     .SetSuccess(false)
        //                     .SetStatusCode((int)HttpStatusCode.BadRequest)
        //                     .SetMessage(ex.Message);

        //             return StatusCode((int)response.StatusCode, response);
        //         }

        //         response = Response<string>.Builder()
        //                     .SetSuccess(false)
        //                     .SetStatusCode((int)HttpStatusCode.InternalServerError)
        //                     .SetMessage("Internal Server Error");

        //         return StatusCode((int)response.StatusCode, response);
        //     }

        // }

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

                await _matchDetailService.UpdateChallengerReady(updateData);

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
        public async Task<ActionResult<Response<string>>> UpdateResult(Guid matchId, [FromBody] MatchDetailUpdateResultDTO updateResultDto)
        {
            var response = new Response<string>();
            try
            {
                var userRawId = this.GetUserId();

                if (userRawId == null) throw new UnauthorizedException("Not allow to access");

                Guid userId = Guid.Parse(userRawId);

                await _matchDetailService.UpdateResultMatch(matchId, updateResultDto, userId);

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