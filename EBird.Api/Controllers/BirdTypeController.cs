﻿using AutoMapper;
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
        public async Task<ActionResult<Response<List<BirdTypeResponseDTO>>>> Get()
        {
            Response<List<BirdTypeResponseDTO>> response;
            try
            {
                var birdTypeResponeList = await _birdTypeService.GetAllBirdType();

                response = new Response<List<BirdTypeResponseDTO>>()
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
                    response = Response<List<BirdTypeResponseDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<List<BirdTypeResponseDTO>>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }

        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BirdTypeResponseDTO>>> Get(Guid id)
        {
            Response<BirdTypeResponseDTO> response;
            try
            {
                var responseData = await _birdTypeService.GetBirdType(id);

                response = new Response<BirdTypeResponseDTO>()
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
                    response = Response<BirdTypeResponseDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<BirdTypeResponseDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // POST  create new bird type
        [HttpPost]
        public async Task<ActionResult<Response<Guid>>> Post([FromBody] BirdTypeRequestDTO birdTypeDTO)
        {
            Response<Guid> response = null;
            try
            {
              var id = await _birdTypeService.AddBirdType(birdTypeDTO);

                response = new Response<Guid>()
                            .SetData(id)
                            .SetStatusCode((int) HttpStatusCode.Created)
                            .SetSuccess(true)
                            .SetMessage("Create bird type is successful");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
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
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // Put : update exist bird type
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> Put(Guid id, [FromBody] BirdTypeRequestDTO birdTypeDTO)
        {
            Response<string> response = null;
            try
            {
                await _birdTypeService.UpdateBirdType(id, birdTypeDTO);

                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Update bird type is successful");

                return response;
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }

        // DELETE: delete bird type
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(Guid id)
        {
            Response<string> response = null;
            try
            {
                 await _birdTypeService.DeleteBirdType(id);

                response = new Response<string>()
                            .SetData("")
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Delete bird type is successful");

                return StatusCode((int) response.StatusCode, response);
            }
            catch(Exception ex)
            {

                if(ex is BadRequestException || ex is NotFoundException)
                {
                    response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.BadRequest)
                        .SetMessage(ex.Message);

                    return StatusCode((int) response.StatusCode, response);
                }

                response = Response<string>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");

                return StatusCode((int) response.StatusCode, response);
            }
        }
    }
}
