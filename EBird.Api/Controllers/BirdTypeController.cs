using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Model.BirdType;
using EBird.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Response;
using System.Net;

namespace EBird.Api.Controllers
{
    [Route("bird-type")]
    [ApiController]
    public class BirdTypeController : ControllerBase
    {
        private IBirdTypeService _birdTypeService;

        public BirdTypeController(IBirdTypeService birdTypeService)
        {
            _birdTypeService = birdTypeService;
        }

        // GET all
        [HttpGet("all")]
        public async Task<ActionResult<Response<List<BirdTypeDTO>>>> Get()
        {
            Response<List<BirdTypeDTO>> response;
            try
            {
                var birdTypeResponeList = await _birdTypeService.GetAllBirdType();

                response = new Response<List<BirdTypeDTO>>()
                            .SetData(birdTypeResponeList)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type by code name is successful");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {

                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<List<BirdTypeDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<List<BirdTypeDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }

        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Get(Guid id)
        {
            Response<BirdTypeDTO> response;
            try
            {
                var responseData = await _birdTypeService.GetBirdType(id);

                response = new Response<BirdTypeDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type is successful");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // POST  create new bird type
        [HttpPost]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Post([FromBody] BirdTypeDTO birdTypeDTO)
        {
            Response<BirdTypeDTO> response = null;
            try
            {
                var responseData = await _birdTypeService.AddBirdType(birdTypeDTO);

                response = new Response<BirdTypeDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Create bird type is successful");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // PATCH : update exist bird type
        [HttpPatch("{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Patch(Guid id, [FromBody] BirdTypeDTO birdTypeDTO)
        {
            Response<BirdTypeDTO> response = null;
            try
            {
                var responseData = await _birdTypeService.UpdateBirdType(id, birdTypeDTO);

                response = new Response<BirdTypeDTO>()
                            .SetData(responseData)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Update bird type is successful");

                return response;
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // DELETE: delete bird type
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<BirdTypeDTO>>> Delete(Guid id)
        {
            Response<BirdTypeDTO> response = null;
            try
            {
                var birdTypeReponse = await _birdTypeService.DeleteBirdType(id);

                response = new Response<BirdTypeDTO>()
                            .SetData(birdTypeReponse)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Delete bird type is successful");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {

                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }
    }
}
