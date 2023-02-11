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
            await _repository.BirdType.DeleteSoftAsync(birdTypeID);
        }

        public async Task DeleteBirdType(string birdTypeCode)
        {
            if(birdTypeCode == null || birdTypeCode.Trim().Length == 0)
            {
                throw new BadRequestException("Invalid bird type code");
            }

            await _repository.BirdType.SoftDeleteAsync(birdTypeCode);
        }

        public async Task<List<BirdTypeResponseDTO>> GetAllBirdType()
        {
            var listBirdType = await _repository.BirdType.GetAllBirdTypeActiveAsync();
            
             return _mapper.Map<List<BirdTypeEntity>, List<BirdTypeResponseDTO>>(listBirdType);
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
                throw new BadRequestException("Bird type not found");
            }

            var birdTypeResultDTO = _mapper.Map<BirdTypeResponseDTO>(birdTypeResult);

            return birdTypeResultDTO;
        }

        public async Task<BirdTypeResponseDTO> GetBirdType(Guid birdTypeID)
        {
            var birdTypeResult = await _repository.BirdType.GetBirdTypeActiveAsync(birdTypeID);

            var birdTypeResultDTO = _mapper.Map<BirdTypeResponseDTO>(birdTypeResult);

            return birdTypeResultDTO;
        }

        public async Task<Guid> AddBirdType(BirdTypeRequestDTO birdTypeDTO)
        {
            await BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO, _repository);

            var birdType = _mapper.Map<BirdTypeEntity>(birdTypeDTO);

            await _repository.BirdType.AddBirdTypeAsync(birdType);
            
            return birdType.Id;
        }
        
        public async Task UpdateBirdType(Guid id, BirdTypeRequestDTO birdTypeDTO)
        {
            await BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO, _repository);

            var birdType = await _repository.BirdType.GetByIdAsync(id);

            if(birdType == null)
            {
                throw new BadRequestException("Can not found bird type for updating");
            }

            _mapper.Map(birdTypeDTO, birdType);

            await _repository.BirdType.UpdateBirdTypeAsync(birdType);
        }
    }
}
