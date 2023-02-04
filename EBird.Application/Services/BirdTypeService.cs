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

        public async Task<BirdTypeDTO> DeleteBirdType(Guid birdTypeID)
        {
                var birdTypeDeleted = await _repository.BirdType.DeleteSoftAsync(birdTypeID);

                if(birdTypeDeleted == null)
                {
                    throw new NotFoundException("Bird type not found");
                }

                var birdTypeDeletedDTO = _mapper.Map<BirdTypeDTO>(birdTypeDeleted);

                return birdTypeDeletedDTO;   
        }

        public async Task<BirdTypeDTO> DeleteBirdType(string birdTypeCode)
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

            return birdTypeDeletedDTO;
        }

        public async Task<List<BirdTypeDTO>> GetAllBirdType()
        {
            var listBirdType = await _repository.BirdType.GetAllBirdTypeActiveAsync();

            if(listBirdType.Count == 0 || listBirdType == null)
            {
                throw new NotFoundException("Bird type not found");
            }

            var listBirdTypeDTO = _mapper.Map<List<BirdTypeDTO>>(listBirdType);

            return listBirdTypeDTO;
        }

        public async Task<BirdTypeDTO> GetBirdType(string birdTypeCode)
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

            var birdTypeResultDTO = _mapper.Map<BirdTypeDTO>(birdTypeResult);

            return birdTypeResultDTO;
        }

        public async Task<BirdTypeDTO> GetBirdType(Guid birdTypeID)
        {
            var birdTypeResult = await _repository.BirdType.GetBirdTypeActiveAsync(birdTypeID);

            if(birdTypeResult == null)
            {
                throw new NotFoundException("Bird type not found");
            }

            var birdTypeResultDTO = _mapper.Map<BirdTypeDTO>(birdTypeResult);

            return birdTypeResultDTO;
        }

        public async Task<BirdTypeDTO> AddBirdType(BirdTypeDTO birdTypeDTO)
        {
            await BirdTypeValidation.ValidateBirdTypeDTO(birdTypeDTO, _repository);

            var birdType = _mapper.Map<BirdTypeEntity>(birdTypeDTO);

            var updatedEntity = await _repository.BirdType.AddBirdTypeAsync(birdType);

            if(updatedEntity == null)
            {
                throw new Exception("Can insert data to database");
            }

            return birdTypeDTO;
        }

        public async Task<BirdTypeDTO> UpdateBirdType(Guid id, BirdTypeDTO birdTypeDTO)
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

            return birdTypeDTO;
        }
    }
}
