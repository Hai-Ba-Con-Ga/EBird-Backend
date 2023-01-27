using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
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
                BirdEntity birdEntity = _mapper.Map<BirdEntity>(birdDTO);
                birdEntity = await _repository.Bird.AddBirdAsync(birdEntity);
                if(birdEntity == null)
                {
                    throw new BadRequestException("Bird not added");
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
                return Response<BirdDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal Server Error");
            }
        }

        public Task<Response<BirdDTO>> DeleteBird(Guid birdID)
        {
            throw new NotImplementedException();
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

        public Task<Response<BirdDTO>> UpdateBird(Guid id, BirdDTO birdDTO)
        {
            throw new NotImplementedException();
        }
    }
}
