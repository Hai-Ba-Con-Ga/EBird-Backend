using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.Match;
using EBird.Application.Model.PagingModel;
using EBird.Application.Services.IServices;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Response;

namespace EBird.Api.Controllers
{
    [ApiController]
    [Route("match")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        //create match from request
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<Guid>>> CreateFromRequest([FromBody] MatchCreateDTO matchCreateDTO)
        {
            var response = new Response<Guid>();
            try
            {
                var userIdRaw = this.GetUserId();

                if (userIdRaw == null) throw new UnauthorizedException("Not allowed to access");

                matchCreateDTO.UserId = Guid.Parse(userIdRaw);

                var matchId = await _matchService.CreateMatchFromRequest(matchCreateDTO);

                response = Response<Guid>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Create match is success")
                    .SetData(matchId);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is NotFoundException ||
                    ex is UnauthorizedException)
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

        //get match by id
        [HttpGet("{matchId}")]
        public async Task<ActionResult<Response<MatchResponseDTO>>> Get(Guid matchId)
        {
            var response = new Response<MatchResponseDTO>();
            try
            {
                var match = await _matchService.GetMatch(matchId);

                response = Response<MatchResponseDTO>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get match is success")
                    .SetData(match);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<MatchResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<MatchResponseDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        //get all match
        [HttpGet("all")]
        public async Task<ActionResult<Response<IList<MatchResponseDTO>>>> GetAll([FromQuery] MatchParameters queryParameters)
        {
            var response = new Response<IList<MatchResponseDTO>>();
            try
            {
                IList<MatchResponseDTO> matches = null;

                matches = await _matchService.GetMatches(queryParameters);

                PagingData metaData = new PagingData()
                {
                    CurrentPage = ((PagedList<MatchResponseDTO>)matches).CurrentPage,
                    PageSize = ((PagedList<MatchResponseDTO>)matches).PageSize,
                    TotalCount = ((PagedList<MatchResponseDTO>)matches).TotalCount,
                    TotalPages = ((PagedList<MatchResponseDTO>)matches).TotalPages,
                    HasNext = ((PagedList<MatchResponseDTO>)matches).HasNext,
                    HasPrevious = ((PagedList<MatchResponseDTO>)matches).HasPrevious
                };

                response = ResponseWithPaging<IList<MatchResponseDTO>>.Builder()
                .SetSuccess(true)
                .SetStatusCode((int)HttpStatusCode.OK)
                .SetMessage("Get all match is successful")
                .SetData(matches)
                .SetPagingData(metaData);


                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<IList<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }
                Console.WriteLine($"Error: {ex.Message} + {ex.StackTrace}");
                
                response = Response<IList<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        //get all match
        [HttpGet("owner/{rolePlayer}/{matchStatus}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Response<ICollection<MatchResponseDTO>>>> GetOwnerWithStatus(string rolePlayer, string matchStatus)
        {
            var response = new Response<ICollection<MatchResponseDTO>>();
            try
            {
                var userIdRaw = this.GetUserId();
                if (userIdRaw == null) throw new UnauthorizedException("Not allowed to access");
                Guid userId = Guid.Parse(userIdRaw);

                ICollection<MatchResponseDTO> matches = null;

                matches = await _matchService.GetWithOwnerAndStatus(userId, rolePlayer, matchStatus);

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all match is success")
                    .SetData(matches);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is NotFoundException ||
                    ex is UnauthorizedException)
                {
                    response = Response<ICollection<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        //delete match
        [HttpDelete("{matchId}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid matchId)
        {
            var response = new Response<string>();
            try
            {
                await _matchService.DeleteMatch(matchId);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Delete match is success")
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

        //update match
        [HttpPut("{matchId}")]
        public async Task<ActionResult<Response<string>>> Put(Guid matchId, [FromBody] MatchUpdateDTO matchUpdateDTO)
        {
            var response = new Response<string>();
            try
            {
                await _matchService.UpdateMatch(matchId, matchUpdateDTO);

                response = Response<string>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Update match is success")
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

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<Response<ICollection<MatchResponseDTO>>>> GetByGroup(Guid groupId)
        {
            var response = new Response<ICollection<MatchResponseDTO>>();
            try
            {
                ICollection<MatchResponseDTO> matches = null;

                matches = await _matchService.GetMatchByGroupId(groupId);

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all match is success")
                    .SetData(matches);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is NotFoundException ||
                    ex is UnauthorizedException)
                {
                    response = Response<ICollection<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }

        [HttpGet("bird/{birdId}")]
        public async Task<ActionResult<Response<ICollection<MatchResponseDTO>>>> GetByBird(Guid birdId, [FromQuery] string? matchStatus)
        {
            var response = new Response<ICollection<MatchResponseDTO>>();
            try
            {
                ICollection<MatchResponseDTO> matches = null;

                matches = await _matchService.GetMatchesByBirdId(birdId, matchStatus);

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all match within birdId is success")
                    .SetData(matches);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException ||
                    ex is NotFoundException ||
                    ex is UnauthorizedException)
                {
                    response = Response<ICollection<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }



    }
}