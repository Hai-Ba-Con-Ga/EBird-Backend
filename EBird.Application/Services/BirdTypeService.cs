using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using Microsoft.AspNetCore.Http;
using EBird.Application.Exceptions;
using System.Net;
using EBird.Application.Validation;
using AutoMapper;

namespace EBird.Application.Services
{
    public class BirdTypeService : IBirdTypeService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;

        public BirdTypeService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<BirdTypeDTO>> DeleteBirdType(Guid birdTypeID)
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

                var birdTypeDeletedDTO = _mapper.Map<BirdTypeDTO>(birdTypeDeleted);

                return new Response<BirdTypeDTO>()
                            .SetSuccess(true)
                            .SetMessage("Soft delete is successfull")
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetData(birdTypeDeletedDTO);
            }
            catch(Exception ex)
            {
                Response<BirdTypeDTO> respone = new Response<BirdTypeDTO>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }

        }

        public async Task<Response<BirdTypeDTO>> DeleteBirdType(string birdTypeCode)
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

                var birdTypeDeletedDTO = _mapper.Map<BirdTypeDTO>(birdTypeDeleted);

                return new Response<BirdTypeDTO>()
                            .SetSuccess(true)
                            .SetMessage("Soft delete is successfull")
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetData(birdTypeDeletedDTO);
            }
            catch(Exception ex)
            {
                Response<BirdTypeDTO> respone = new Response<BirdTypeDTO>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public async Task<Response<List<BirdTypeDTO>>> GetAllBirdType()
        {
            var listBirdType = await _repository.BirdType.GetAllAsync();

            var listBirdTypeDTO = _mapper.Map<List<BirdTypeDTO>>(listBirdType);

            return new Response<List<BirdTypeDTO>>()
                        .SetData(listBirdTypeDTO)
                        .SetMessage("Get all of bird type is succesful")
                        .SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK);
        }

        public async Task<Response<BirdTypeDTO>> GetBirdType(string birdTypeCode)
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

                var birdTypeResultDTO = _mapper.Map<BirdTypeDTO>(birdTypeResult);
                
                return new Response<BirdTypeDTO>()
                            .SetData(birdTypeResultDTO)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type by code name is successful");
            }
            catch(Exception ex)
            {

                Response<BirdTypeDTO> respone = new Response<BirdTypeDTO>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public async Task<Response<BirdTypeDTO>> GetBirdType(Guid birdTypeID)
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

                var birdTypeResultDTO = _mapper.Map<BirdTypeDTO>(birdTypeResult);
                
                return new Response<BirdTypeDTO>()
                            .SetData(birdTypeResultDTO)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type by code name is successful");
            }
            catch(Exception ex)
            {

                Response<BirdTypeDTO> respone = new Response<BirdTypeDTO>();
                if(ex is BadHttpRequestException || ex is NotFoundException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public async Task<Response<BirdTypeDTO>> InsertBirdType(BirdTypeDTO birdTypeDTO)
        {
            try
            {
                if(birdTypeDTO == null)
                {
                    throw new BadHttpRequestException("Invalid bird type");
                }

                var birdType = _mapper.Map<BirdTypeEntity>(birdTypeDTO);

                bool isValid = BirdTypeValidation.ValidateBirdTypeEntity(birdType);
                
                int rowEffect = await _repository.BirdType.CreateAsync(birdType);
                
                if(rowEffect == 0)
                {
                    throw new Exception("Can insert data to database");
                }

                return new Response<BirdTypeDTO>()
                            .SetData(birdTypeDTO)
                            .SetSuccess(true)
                            .SetMessage("Insert bird type is successful")
                            .SetStatusCode((int) HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                Response<BirdTypeDTO> respone = new Response<BirdTypeDTO>();
                if(ex is BadHttpRequestException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }

        public async Task<Response<BirdTypeDTO>> UpdateBirdType(Guid id, BirdTypeDTO birdTypeDTO)
        {
            try
            {
                var birdType = await _repository.BirdType.GetByIdAsync(id);

                _mapper.Map(birdTypeDTO, birdType);

                bool isValid = BirdTypeValidation.ValidateBirdTypeEntity(birdType);
                
                if(isValid == false)
                {
                    throw new BadHttpRequestException("Invalid bird type entity");
                }
                
                int rowEffect = await _repository.BirdType.UpdateAsync(birdType);
                
                if(rowEffect == 0)
                {
                    throw new Exception("Can update data to database");
                }
                
                return new Response<BirdTypeDTO>()
                            .SetData(birdTypeDTO)
                            .SetSuccess(true)
                            .SetMessage("Update bird type is successful")
                            .SetStatusCode((int) HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                Response<BirdTypeDTO> respone = new Response<BirdTypeDTO>();
                if(ex is BadHttpRequestException)
                {
                    respone.SetStatusCode(((BaseHttpException) ex).StatusCode);
                }
                return respone.SetSuccess(false)
                                .SetMessage(ex.Message);
            }
        }
        
    }
}
