using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Application.Interfaces;
using EBird.Application.Exceptions;
using System.Net;
using EBird.Application.Validation;
using AutoMapper;
using Microsoft.Data.SqlClient;
using EBird.Application.Model.BirdType;

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

        public async Task DeleteBirdType(Guid birdTypeID)
        {
                var birdTypeDeleted = await _repository.BirdType.DeleteSoftAsync(birdTypeID);

                if(birdTypeDeleted == null)
                {
                    throw new NotFoundException("Bird type not found");
                }   
        }

        public async Task DeleteBirdType(string birdTypeCode)
        {
            if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
            {
                throw new BadRequestException("Invalid bird type code");
            }

            var isSuccess = await _repository.BirdType.SoftDeleteAsync(birdTypeCode);

            if(isSuccess == false)
            {
                throw new BadRequestException("Can not delete bird type");
            }
        }

        public async Task<List<BirdTypeResponseDTO>> GetAllBirdType()
        {
            var listBirdType = await _repository.BirdType.GetAllBirdTypeActiveAsync();
            
             return _mapper.Map<List<BirdTypeResponseDTO>>(listBirdType);
        }

        public async Task<BirdTypeResponseDTO> GetBirdType(string birdTypeCode)
        {
            if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
            {
                throw new BadRequestException("Invalid bird type code");
            }

            var birdTypeResult = await _repository.BirdType.GetBirdTypeByCodeAsync(birdTypeCode);

            if(birdTypeResult == null)
            {
                throw new NotFoundException("Bird type not found");
            }

            var birdTypeResultDTO = _mapper.Map<BirdTypeResponseDTO>(birdTypeResult);

            return birdTypeResultDTO;
        }

        public async Task<BirdTypeResponseDTO> GetBirdType(Guid birdTypeID)
        {
            var birdTypeResult = await _repository.BirdType.GetBirdTypeActiveAsync(birdTypeID);

            if(birdTypeResult == null)
            {
                throw new NotFoundException("Bird type not found");
            }

            var birdTypeResultDTO = _mapper.Map<BirdTypeResponseDTO>(birdTypeResult);

            return birdTypeResultDTO;
        }

        public async Task AddBirdType(BirdTypeRequestDTO birdTypeDTO)
        {
            await BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO, _repository);

            var birdType = _mapper.Map<BirdTypeEntity>(birdTypeDTO);

            bool isSuccess = await _repository.BirdType.AddBirdTypeAsync(birdType);
            
            if(isSuccess == false)
            {
                throw new Exception("Can insert data to database");
            }
        }
        
        public async Task UpdateBirdType(Guid id, BirdTypeRequestDTO birdTypeDTO)
        {
            await BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO, _repository);

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
        }
    }
}
