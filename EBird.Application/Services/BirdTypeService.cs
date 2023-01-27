using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Exceptions;
using System.Net;
using EBird.Application.Validation;
using AutoMapper;
using Microsoft.Data.SqlClient;

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
                if(ex is NotFoundException)
                {
                    return Response<BirdTypeDTO>.Builder()
                                .SetSuccess(false)
                                .SetStatusCode(((BaseHttpException) ex).StatusCode)
                                .SetMessage(ex.Message);
                }
                //handle other exception
                Console.WriteLine(ex.Message);
                return Response<BirdTypeDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal server error");
            }
        }

        public async Task<Response<BirdTypeDTO>> DeleteBirdType(string birdTypeCode)
        {
            try
            {
                if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
                {
                    throw new BadRequestException("Invalid bird type code");

                }

                var birdTypeDeleted = await _repository.BirdType.SoftDeleteAsync(birdTypeCode);

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
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    return Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);
                }
                Console.WriteLine(ex.Message);
                return Response<BirdTypeDTO>.Builder()
                                .SetSuccess(false)
                                .SetStatusCode((int) HttpStatusCode.InternalServerError)
                                .SetMessage("Internal server error");
            }
        }

        public async Task<Response<List<BirdTypeDTO>>> GetAllBirdType()
        {
            try
            {
                var listBirdType = await _repository.BirdType.GetAllBirdTypeActiveAsync();

                if(listBirdType.Count == 0 || listBirdType == null)
                {
                    throw new NotFoundException("Bird type not found");
                }

                var listBirdTypeDTO = _mapper.Map<List<BirdTypeDTO>>(listBirdType);

                return new Response<List<BirdTypeDTO>>()
                            .SetData(listBirdTypeDTO)
                            .SetMessage("Get all of bird type is succesful")
                            .SetSuccess(true).SetStatusCode((int) HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                if(ex is NotFoundException)
                {
                    return Response<List<BirdTypeDTO>>.Builder()
                                .SetSuccess(false)
                                .SetStatusCode(((BaseHttpException) ex).StatusCode)
                                .SetMessage(ex.Message);
                }
                //handle other exception
                Console.WriteLine(ex.Message);
                return Response<List<BirdTypeDTO>>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode((int) HttpStatusCode.InternalServerError)
                            .SetMessage("Internal server error");
            }
        }

        public async Task<Response<BirdTypeDTO>> GetBirdType(string birdTypeCode)
        {
            try
            {
                if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
                {
                    throw new BadRequestException("Invalid bird type code");
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
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    return Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode(((BaseHttpException) ex).StatusCode)
                        .SetMessage(ex.Message);
                }
                Console.WriteLine(ex.Message);
                return Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
        }

        public async Task<Response<BirdTypeDTO>> GetBirdType(Guid birdTypeID)
        {
            try
            {
                var birdTypeResult = await _repository.BirdType.GetBirdTypeActiveAsync(birdTypeID);

                if(birdTypeResult == null)
                {
                    throw new NotFoundException("Bird type not found");
                }

                var birdTypeResultDTO = _mapper.Map<BirdTypeDTO>(birdTypeResult);

                return new Response<BirdTypeDTO>()
                            .SetData(birdTypeResultDTO)
                            .SetStatusCode((int) HttpStatusCode.OK)
                            .SetSuccess(true)
                            .SetMessage("Get bird type is successful");
            }
            catch(Exception ex)
            {
                if(ex is NotFoundException)
                {
                    return Response<BirdTypeDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                Console.WriteLine(ex.Message);
                return Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
        }

        public async Task<Response<BirdTypeDTO>> AddBirdType(BirdTypeDTO birdTypeDTO)
        {
            try
            {
                bool isValid = BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO);
                if(birdTypeDTO == null || isValid == false)
                {
                    throw new BadRequestException("Invalid bird type");
                }

                bool isValidTypeCode = BirdTypeValidation.ValidateBirdTypeCode(_repository, birdTypeDTO.TypeCode);
                if(isValidTypeCode == false)
                {
                    throw new BadRequestException("Invalid bird type code");
                }

                var birdType = _mapper.Map<BirdTypeEntity>(birdTypeDTO);
                birdType.CreatedDatetime = DateTime.Now;

                if(isValid == false)
                {
                    throw new BadRequestException("Invalid bird type");
                }

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
                if(ex is BadRequestException)
                {
                    return Response<BirdTypeDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                //if(ex is SqlException)
                Console.WriteLine(ex.Message);
                return Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
        }

        public async Task<Response<BirdTypeDTO>> UpdateBirdType(Guid id, BirdTypeDTO birdTypeDTO)
        {
            try
            {
                bool isValid = BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO);

                if(isValid == false)
                {
                    throw new BadRequestException("Invalid bird type for updating");
                }

                var birdType = await _repository.BirdType.GetByIdAsync(id);

                if(birdType == null)
                {
                    throw new NotFoundException("Can not found bird type for updating");
                }

                _mapper.Map(birdTypeDTO, birdType);

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
                if(ex is BadRequestException || ex is NotFoundException)
                {
                    return Response<BirdTypeDTO>.Builder()
                            .SetSuccess(false)
                            .SetStatusCode(((BaseHttpException) ex).StatusCode)
                            .SetMessage(ex.Message);
                }
                return Response<BirdTypeDTO>.Builder()
                        .SetSuccess(false)
                        .SetStatusCode((int) HttpStatusCode.InternalServerError)
                        .SetMessage("Internal server error");
            }
        }

    }
}
