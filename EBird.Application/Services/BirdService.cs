using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class BirdService : IBirdService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;

        public BirdService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<BirdDTO>> AddBird(BirdDTO birdDTO)
        {
            try
            {
                await BirdValidation.ValidateBird(birdDTO, _repository);
                
                BirdEntity birdEntity = _mapper.Map<BirdEntity>(birdDTO);
                
                birdEntity = await _repository.Bird.AddBirdAsync(birdEntity);
                
                if(birdEntity == null)
                {
                    throw new BadRequestException("Bird is not added");
                }
                return Response<BirdDTO>.Builder()
                           .SetSuccess(true)
                           .SetStatusCode((int) HttpStatusCode.OK)
                           .SetMessage("Add bird successful")
                           .SetData(birdDTO);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                Console.WriteLine(ex.Message);
                return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");
            }
        }

        public async Task<Response<BirdDTO>> DeleteBird(Guid birdID)
        {
            try
            {
                var birdEntity = await _repository.Bird.SoftDeleteBirdAsync(birdID);
                if(birdEntity == null)
                {
                    throw new NotFoundException("Not found Bird for delete");
                }
                return Response<BirdDTO>.Builder()
                           .SetSuccess(true)
                           .SetStatusCode((int) HttpStatusCode.OK)
                           .SetMessage("Delete bird successful");
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                return Response<BirdDTO>.Builder()
                           .SetSuccess(false)
                           .SetStatusCode((int) HttpStatusCode.InternalServerError)
                           .SetMessage("Internal Server Error");
            }
        }

        public async Task<Response<BirdDTO>> GetBird(Guid birdID)
        {
            try
            {
                var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdID);
                if(birdEntity == null)
                {
                    throw new NotFoundException("Can not found bird");
                }
                var birdDTO = _mapper.Map<BirdDTO>(birdEntity);
                return Response<BirdDTO>.Builder()
                           .SetSuccess(true)
                           .SetStatusCode((int) HttpStatusCode.OK)
                           .SetMessage("Get bird successful")
                           .SetData(birdDTO);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                Console.WriteLine(ex.Message);
                return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");
            }
        }

        public async Task<Response<List<BirdDTO>>> GetBirds()
        {
            try
            {
                var birdEntity = await _repository.Bird.GetBirdsActiveAsync();
                if(birdEntity.Count == 0)
                {
                    throw new NotFoundException("Can not found bird");
                }
                var birdDTO = _mapper.Map<List<BirdDTO>>(birdEntity);
                return Response<List<BirdDTO>>.Builder()
                           .SetSuccess(true)
                           .SetStatusCode((int) HttpStatusCode.OK)
                           .SetMessage("Get bird successful")
                           .SetData(birdDTO);
            }
            catch(Exception ex)
            {
                if(ex is BadRequestException)
                {
                    return Response<List<BirdDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                return Response<List<BirdDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");
            }
        }

        public async Task<Response<BirdDTO>> UpdateBird(Guid birdID, BirdDTO birdDTO)
        {
            try
            {
                await BirdValidation.ValidateBird(birdDTO, _repository);
                
                var birdEntity = await _repository.Bird.GetBirdActiveAsync(id);

                if(birdEntity == null)
                {
                    throw new NotFoundException("Can not found bird");
                }

                _mapper.Map(birdDTO, birdEntity);

                var result = await _repository.Bird.UpdateBirdAsync(birdEntity);

                if(result == null)
                {
                    throw new BadRequestException("Do not have data change");
                }

                return Response<BirdDTO>.Builder()
                           .SetSuccess(true)
                           .SetStatusCode((int) HttpStatusCode.OK)
                           .SetMessage("Update bird successful")
                           .SetData(birdDTO);
            }
            catch(Exception ex)
            {
                if(ex is NotFoundException || ex is BadRequestException)
                {
                    return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                Console.WriteLine(ex.Message);
                return Response<BirdDTO>.Builder()
                           .SetSuccess(false)
                           .SetStatusCode((int) HttpStatusCode.InternalServerError)
                           .SetMessage("Internal Server Error");
            }
        }

    }
}
