using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Match;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Services
{
    public class MatchDeatailService : IMatchDetailService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;
        private IUnitOfValidation _unitOfValidation;
        private IGenericRepository<ResourceEntity> _resourceRepository;
        private IGenericRepository<MatchResourceEntity> _matchResourceRepository;
        private IGenericRepository<MatchDetailEntity> _matchBirdEntityRepository;
        public MatchDeatailService(IWapperRepository repository, IMapper mapper,
        IUnitOfValidation unitOfValidation, IGenericRepository<ResourceEntity> resourceRepository,
        IGenericRepository<MatchResourceEntity> matchResourceRepository,
        IGenericRepository<MatchDetailEntity> matchBirdEntityRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfValidation = unitOfValidation;
            _resourceRepository = resourceRepository;
            _matchResourceRepository = matchResourceRepository;
            _matchBirdEntityRepository = matchBirdEntityRepository;
        }

        public async Task UpdateBirdInMatch(MatchDetailUpdateDTO matchBirdUpdateDTO)
        {
            var matchBird = await _repository.MatchBird.GetByIdActiveAsync(matchBirdUpdateDTO.MatchBirdId);

            if (matchBird == null)
            {
                throw new BadRequestException("Match Bird not found");
            }
            if (matchBird.MatchId != matchBirdUpdateDTO.MatchId || matchBird.BirdId != matchBirdUpdateDTO.OldBirdId)
            {
                throw new BadRequestException("Information update does not match");
            }

            matchBird.BirdId = matchBirdUpdateDTO.NewBirdId;

            await _repository.MatchBird.UpdateAsync(matchBird);
        }

        public async Task UpdateChallengerReady(UpdateChallengerToReadyDTO updateData)
        {
            var match = await _repository.Match.GetByIdActiveAsync(updateData.MatchId);

            await _unitOfValidation.Base.ValidateMatchId(updateData.MatchId);

            if (match == null) throw new BadRequestException("Match not found");

            if (match.ChallengerId != updateData.ChallengerId)
                throw new BadRequestException("This match not belong to this challenger");

            var bird = await _repository.Bird.GetByIdActiveAsync(updateData.BirdId);

            if (bird.OwnerId != updateData.ChallengerId)
                throw new BadRequestException("This bird not belong to this challenger");

            await _repository.MatchBird.UpdateMatchBird(updateData);
        }

        public async Task UpdateMatchResult(UpdateMatchResultDTO matchResultDTO)
        {
            var matchBird = await _matchBirdEntityRepository.GetByIdActiveAsync(matchResultDTO.MatchBirdId);
            if (matchBird == null)
            {
                throw new BadRequestException("Match Bird not found");
            }
            int effectRow;
            matchBird.Result = matchResultDTO.Result;
            await _matchBirdEntityRepository.UpdateAsync(matchBird);
            if (matchResultDTO.ListResource != null)
            {
                foreach (var resource in matchResultDTO.ListResource)
                {
                    var resourceEntity = _mapper.Map<ResourceEntity>(resource);
                    effectRow = await _resourceRepository.CreateAsync(resourceEntity);
                    if (effectRow == 0)
                    {
                        throw new BadRequestException("Create resource failed");
                    }
                    var matchResource = new MatchResourceEntity
                    {
                        MatchBirdId = matchBird.Id,
                        ResourceId = resourceEntity.Id
                    };
                    effectRow = await _matchResourceRepository.CreateAsync(matchResource);
                    if (effectRow == 0)
                    {
                        throw new BadRequestException("Create match resource failed");
                    }
                }
            }
        }

        public async Task UpdateResultMatch(Guid matchId, MatchDetailUpdateResultDTO updateResultData, Guid userId)
        {
            await _unitOfValidation.Base.ValidateMatchId(matchId);
            await _unitOfValidation.Base.ValidateBirdId(updateResultData.BirdId);
            await _unitOfValidation.Base.ValidateAccountId(userId);

            var match = await _repository.Match.GetMatch(matchId);

            if (match.MatchStatus != MatchStatus.During)
                throw new BadRequestException("Match not in during status");

            if (match.ChallengerId != userId && match.HostId != userId)
            {
                throw new BadRequestException("User not in this match");
            }

            await _repository.MatchBird.UpdateResultMatch(matchId, updateResultData.BirdId, updateResultData.Result);
        }
    }
}