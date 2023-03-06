using EBird.Api.Controllers.Exentions;
using EBird.Application.Exceptions;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
using EBird.Application.Services.IServices;
using EBird.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Response;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBird.Api.Controllers
{
    [Route("statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IBirdService _birdService;

        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(RoleAccount.Admin))]
        [HttpGet("bird")]
        public async Task<ActionResult<Response<IList<BirdResponseDTO>>>> Get([FromQuery] BirdParameters parameters)
        {
            Response<IList<BirdResponseDTO>> response = null;
            try
            {   
                IList<BirdResponseDTO> listBirdDTO = null;

                listBirdDTO = await _birdService.GetBirdsByPagingParameters(parameters);

                PagingData metaData = new PagingData()
                {
                    CurrentPage = ((PagedList<BirdResponseDTO>)listBirdDTO).CurrentPage,
                    PageSize = ((PagedList<BirdResponseDTO>)listBirdDTO).PageSize,
                    TotalCount = ((PagedList<BirdResponseDTO>)listBirdDTO).TotalCount,
                    TotalPages = ((PagedList<BirdResponseDTO>)listBirdDTO).TotalPages,
                    HasNext = ((PagedList<BirdResponseDTO>)listBirdDTO).HasNext,
                    HasPrevious = ((PagedList<BirdResponseDTO>)listBirdDTO).HasPrevious
                };

                response = ResponseWithPaging<IList<BirdResponseDTO>>.Builder()
                .SetSuccess(true)
                .SetStatusCode((int)HttpStatusCode.OK)
                .SetMessage("Get all birds successful")
                .SetData(listBirdDTO)
                .SetPagingData(metaData);

                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<IList<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.BadRequest)
                            .SetMessage(ex.Message);

                    return StatusCode((int)response.StatusCode, response);
                }

                response = Response<IList<BirdResponseDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int)HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");

                return StatusCode((int)response.StatusCode, response);
            }
        }


    }
}
