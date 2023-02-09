using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
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

        public async Task AddBird(BirdCreateDTO birdDTO)
        {
            await BirdValidation.ValidateCreateBird(birdDTO, _repository);

            BirdEntity birdEntity = _mapper.Map<BirdEntity>(birdDTO);

            var resourceDTOList = birdDTO.ListResource;

            if (resourceDTOList == null)
            {
                await _repository.Bird.AddBirdAsync(birdEntity);
            }
            else
            {
                await BirdValidation.ValidateCreateResourceList(resourceDTOList, _repository);

                var resourceEntityList = _mapper.Map<List<ResourceEntity>>(resourceDTOList);

                await _repository.Bird.AddBirdAsync(birdEntity, resourceEntityList);
            }
        }

        public async Task DeleteBird(Guid birdID)
        {
            await _repository.Bird.SoftDeleteBirdAsync(birdID);
        }

        public async Task<BirdResponseDTO> GetBird(Guid birdID)
        {
            var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdID);

            if (birdEntity == null)
            {
                throw new NotFoundException("Can not found bird");
            }

            var birdDTO = _mapper.Map<BirdResponseDTO>(birdEntity);

            birdDTO.ResourceList = await _repository.Resource.GetResourcesByBird(birdDTO.Id);
            
            return birdDTO;
        }

        public async Task<List<BirdResponseDTO>> GetBirds()
        {
            var listBirdEntity = await _repository.Bird.GetBirdsActiveAsync();

            if (listBirdEntity.Count == 0)
            {
                throw new NotFoundException("Can not found bird");
            }

            var listBirdDTO = _mapper.Map<List<BirdResponseDTO>>(listBirdEntity);

            foreach (var birdDto in listBirdDTO)
            {
                birdDto.ResourceList = await _repository.Resource.GetResourcesByBird(birdDto.Id);
            }

            return listBirdDTO;
        }

        public async Task<PagedList<BirdResponseDTO>> GetBirdsByPagingParameters(BirdParameters parameters)
        {
            BirdValidation.ValidateBirdParameter(parameters);

            var birdList = await _repository.Bird.GetBirdsActiveAsync(parameters);

            var birdListDTO = _mapper.Map<PagedList<BirdResponseDTO>>(birdList);
            birdListDTO.MapMetaData(birdList);

            foreach (var birdDto in birdListDTO)
            {
                birdDto.ResourceList = await _repository.Resource.GetResourcesByBird(birdDto.Id);
            }

            return birdListDTO;
        }

        public async Task UpdateBird(Guid birdID, BirdRequestDTO birdDTO)
        {
            await BirdValidation.ValidateUpdateBird(birdDTO, _repository);

            var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdID);

            if (birdEntity == null)
            {
                throw new NotFoundException("Can not found bird for update");
            }

            _mapper.Map(birdDTO, birdEntity);

            await _repository.Bird.UpdateBirdAsync(birdEntity);
        }

        public async Task<List<BirdResponseDTO>> GetAllBirdByAccount(Guid accountId)
        {
            var listBirdEntity = await _repository.Bird.GetAllBirdActiveByAccountId(accountId);
            
            if (listBirdEntity.Count == 0)
            {
                throw new NotFoundException("Can not found bird");
            }

            var birdListDto = _mapper.Map<List<BirdResponseDTO>>(listBirdEntity);

            foreach (var birdDto in birdListDto)
            {
                birdDto.ResourceList = await _repository.Resource.GetResourcesByBird(birdDto.Id);
            }

            return birdListDto;
        }

    }
}
