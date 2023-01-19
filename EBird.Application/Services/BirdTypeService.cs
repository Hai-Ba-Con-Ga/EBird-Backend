using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using Microsoft.AspNetCore.Http;
using EBird.Application.Exceptions;
using System.Net;


namespace EBird.Application.Services
{
    public class BirdTypeService : IBirdTypeService
    {
        private IWapperRepository _repository;

        public BirdTypeService(IWapperRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<BirdTypeEntity>> DeleteBirdType(Guid birdTypeID)
        {
            try
            {
                if(birdTypeID == null)
                {
                    throw new BadHttpRequestException("Invalid bird type id");

                }

                var birdTypeDeleted = await _repository.BirdType.DeleteSoftAsync(birdTypeID);

                if(birdTypeDeleted == null)
                {
                    throw new NotFoundException("Bird type not found");
                }

                return new Response<BirdTypeEntity>()
                            .SetSuccess(true)
                            .SetMessage("Soft delete is successfull")
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetData(birdTypeDeleted);
            }
            catch(Exception ex)
            {
                Response<BirdTypeEntity> respone = new Response<BirdTypeEntity>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }

        }

        public async Task<Response<BirdTypeEntity>> DeleteBirdType(string birdTypeCode)
        {
            try
            {
                if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
                {
                    throw new BadHttpRequestException("Invalid bird type code");

                }

                var birdTypeDeleted = await _repository.BirdType.DeleteSoftAsync(birdTypeCode);

                if(birdTypeDeleted == null)
                {
                    throw new NotFoundException("Bird type not found");
                }

                return new Response<BirdTypeEntity>()
                            .SetSuccess(true)
                            .SetMessage("Soft delete is successfull")
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetData(birdTypeDeleted);
            }
            catch(Exception ex)
            {
                Response<BirdTypeEntity> respone = new Response<BirdTypeEntity>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public async Task<Response<List<BirdTypeEntity>>> GetAllBirdType()
        {
            var listBirdType = await _repository.BirdType.GetAllAsync();

            return new Response<List<BirdTypeEntity>>()
                        .SetData(listBirdType)
                        .SetMessage("Get all of bird type is succesful")
                        .SetSuccess(true).SetStatusCode((int)HttpStatusCode.OK);
        }

        public async Task<Response<BirdTypeEntity>> GetBirdType(string birdTypeCode)
        {
            try
            {
                if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
                {
                    throw new BadHttpRequestException("Invalid bird type code");
                }
                var birdTypeResult = await _repository.BirdType.GetBirdTypeByCode(birdTypeCode);
                if(birdTypeResult == null)
                {
                    throw new NotFoundException("Bird type not found");
                }
                return new Response<BirdTypeEntity>()
                            .SetData(birdTypeResult)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type by code name is successful");
            }
            catch(Exception ex)
            {

                Response<BirdTypeEntity> respone = new Response<BirdTypeEntity>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public async Task<Response<BirdTypeEntity>> GetBirdType(Guid birdTypeID)
        {
            try
            {
                if(birdTypeID == null)
                {
                    throw new BadHttpRequestException("Invalid bird type code");
                }
                var birdTypeResult = await _repository.BirdType.GetByIdAsync(birdTypeID);
                if(birdTypeResult == null)
                {
                    throw new NotFoundException("Bird type not found");
                }
                return new Response<BirdTypeEntity>()
                            .SetData(birdTypeResult)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type by code name is successful");
            }
            catch(Exception ex)
            {

                Response<BirdTypeEntity> respone = new Response<BirdTypeEntity>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public Task<Response<BirdTypeEntity>> InsertBirdType(BirdTypeEntity birdType)
        {
            
        }

        public Task<Response<BirdTypeEntity>> UpdateBirdType(BirdTypeEntity birdType)
        {
            throw new NotImplementedException();
        }
    }
}
