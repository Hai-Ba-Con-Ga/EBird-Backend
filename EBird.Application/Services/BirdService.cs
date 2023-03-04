using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
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
        private IUnitOfValidation _unitOfValidation;
        private IGenericRepository<MatchDetailEntity> _matchBirdRepository;

        public BirdService(IWapperRepository repository, IMapper mapper, IUnitOfValidation unitOfValidation, IGenericRepository<MatchDetailEntity> matchBirdRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfValidation = unitOfValidation;
            _matchBirdRepository = matchBirdRepository;
        }

        public async Task<Guid> AddBird(BirdCreateDTO birdDTO)
        {
            await _unitOfValidation.Bird.ValidateCreateBird(birdDTO);

            BirdEntity birdEntity = _mapper.Map<BirdEntity>(birdDTO);

            var resourceDTOList = birdDTO.ListResource;


            if(resourceDTOList == null)
            {
                await _repository.Bird.AddBirdAsync(birdEntity);
            }
            else
            {
                await _unitOfValidation.Bird.ValidateCreateResourceList(resourceDTOList);

                var resourceEntityList = _mapper.Map<List<ResourceEntity>>(resourceDTOList);

                await _repository.Bird.AddBirdAsync(birdEntity, resourceEntityList);
            }
            return birdEntity.Id;
        }

        public async Task DeleteBird(Guid userId, Guid birdId)
        {
            await _unitOfValidation.Bird.ValidateBirdDelete(userId, birdId);

            await _repository.Bird.SoftDeleteBirdAsync(birdId);
        }

        public async Task<BirdResponseDTO> GetBird(Guid birdID)
        {
            var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdID);

            if(birdEntity == null)
            {
                throw new NotFoundException("Can not found bird");
            }

            var birdDTO = _mapper.Map<BirdResponseDTO>(birdEntity);

            birdDTO.ResourceList = await _repository.Resource.GetResourcesByBird(birdDTO.Id);
            birdDTO.Ratio = await GetBirdRatio(birdID);


            return birdDTO;
        }

        public async Task<List<BirdResponseDTO>> GetBirds()
        {
            var listBirdEntity = await _repository.Bird.GetBirdsActiveAsync();

            if(listBirdEntity.Count == 0)
            {
                throw new NotFoundException("Can not found bird");
            }

            var listBirdDTO = _mapper.Map<List<BirdResponseDTO>>(listBirdEntity);

            foreach(var birdDto in listBirdDTO)
            {
                birdDto.ResourceList = await _repository.Resource.GetResourcesByBird(birdDto.Id);
                birdDto.Ratio = await GetBirdRatio(birdDto.Id);
            }

            return listBirdDTO;
        }

        public async Task<PagedList<BirdResponseDTO>> GetBirdsByPagingParameters(BirdParameters parameters)
        {
            _unitOfValidation.Bird.ValidateParameter(parameters);

            var birdList = await _repository.Bird.GetBirdsActiveAsync(parameters);

            var birdListDTO = _mapper.Map<PagedList<BirdResponseDTO>>(birdList);
            birdListDTO.MapMetaData(birdList);

            foreach(var birdDto in birdListDTO)
            {
                birdDto.ResourceList = await _repository.Resource.GetResourcesByBird(birdDto.Id);
                birdDto.Ratio = await GetBirdRatio(birdDto.Id);
            }

            return birdListDTO;
        }

        public async Task UpdateBird(Guid birdId, BirdRequestDTO birdDTO, Guid userId)
        {
            await _unitOfValidation.Bird.ValidateUpdateBird(birdDTO, birdId, userId);

            var birdEntity = await _repository.Bird.GetBirdActiveAsync(birdId);

            if(birdEntity == null)
            {
                throw new NotFoundException("Can not found bird for update");
            }

            _mapper.Map(birdDTO, birdEntity);

            await _repository.Bird.UpdateBirdAsync(birdEntity);
        }

        public async Task<List<BirdResponseDTO>> GetAllBirdByAccount(Guid accountId)
        {
            var listBirdEntity = await _repository.Bird.GetAllBirdActiveByAccountId(accountId);

            if(listBirdEntity.Count == 0)
            {
                throw new NotFoundException("Can not found bird");
            }

            var birdListDto = _mapper.Map<List<BirdResponseDTO>>(listBirdEntity);

            foreach(var birdDto in birdListDto)
            {
                birdDto.ResourceList = await _repository.Resource.GetResourcesByBird(birdDto.Id);
                birdDto.Ratio = await GetBirdRatio(birdDto.Id);
            }

            return birdListDto;
        }

        private async Task<BirdRatioDTO> GetBirdRatio(Guid birdId)
        {
            var matchBirdList = await _matchBirdRepository.WhereAsync(x => x.BirdId == birdId && x.IsDeleted == false);
            BirdRatioDTO birdRatio = new BirdRatioDTO();
            if(matchBirdList.Count == 0)
            {
                birdRatio.Win = 0;
                birdRatio.Lose = 0;
                birdRatio.Ratio = 0;
                return birdRatio;
            }

            birdRatio.Win = matchBirdList
                            .Where(x => x.Result == MatchDetailResult.Win)
                            .Count();
            birdRatio.Lose = matchBirdList
                            .Where(x => x.Result == MatchDetailResult.Lose)
                            .Count();

            if((birdRatio.Win + birdRatio.Lose) == 0)
            {
                birdRatio.Ratio = 0;
            }
            else
            {
                birdRatio.Ratio = birdRatio.Win / (birdRatio.Win + birdRatio.Lose);
            }
            return birdRatio;
        }


    }
}
