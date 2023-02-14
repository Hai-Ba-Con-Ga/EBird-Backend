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
    [Route("match")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }
        
        //creat match
        [HttpPost]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] MatchCreateDTO matchCreateDTO)
        {
            var response = new Response<Guid>();
            try
            {
                var matchId = await _matchService.CreateMatch(matchCreateDTO);
                
                response = Response<Guid>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Create match is success")
                    .SetData(matchId);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
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
        public async Task<ActionResult<Response<ICollection<MatchResponseDTO>>>> GetAll()
        {
            var response = new Response<ICollection<MatchResponseDTO>>();
            try
            {
                var matches = await _matchService.GetMatches();

                response = Response<ICollection<MatchResponseDTO>>.Builder()
                    .SetSuccess(true)
                    .SetStatusCode((int)HttpStatusCode.OK)
                    .SetMessage("Get all match is success")
                    .SetData(matches);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
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
    }
}