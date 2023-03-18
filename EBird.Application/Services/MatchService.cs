using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Bird;
using EBird.Application.Model.Match;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IWapperRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfValidation _validation;
        private readonly IMatchDetailService _matchDetailService;

        public MatchService(IWapperRepository repository, IMapper mapper, IUnitOfValidation validation, IMatchDetailService matchDetailService)
        {
            _repository = repository;
            _mapper = mapper;
            _validation = validation;
            _matchDetailService = matchDetailService;
        }

        // public async Task<Guid> CreateMatch(MatchCreateDTO matchCreateDTO)
        // {
        //     MatchEntity match = _mapper.Map<MatchEntity>(matchCreateDTO);

        //     MatchDetailEntity matchBird = new MatchDetailEntity()
        //     {
        //         BirdId = matchCreateDTO.BirdHostId,
        //         Result = Domain.Enums.MatchDetailResult.Ready
        //     };

        //     var matchId = await _repository.Match.CreateMatch(match, matchBird);

        //     return matchId;
        // }

        public async Task DeleteMatch(Guid matchId)
        {
            await _repository.Match.DeleteSoftAsync(matchId);
        }

        public async Task<MatchResponseDTO> GetMatch(Guid matchId)
        {
            var match = await _repository.Match.GetMatch(matchId);

            if (match == null) throw new BadRequestException("Match not found");

            var matchDTO = _mapper.Map<MatchResponseDTO>(match);

            matchDTO.MatchDetails = _mapper.Map<ICollection<MatchDetailResponseDTO>>(match.MatchDetails);

            foreach (var matchDetail in matchDTO.MatchDetails)
            {
                var matchResources = await _repository.MatchDetail.GetMatchResources(matchDetail.Id);

                matchDetail.ResourceResponses = _mapper.Map<ICollection<ResourceResponse>>(matchResources);
            }

            return matchDTO;
        }

        public async Task<ICollection<MatchResponseDTO>> GetMatches()
        {
            var collection = await _repository.Match.GetMatches();

            var matchDtoList = _mapper.Map<ICollection<MatchResponseDTO>>(collection);

            foreach (var matchDto in matchDtoList)
            {
                matchDto.MatchDetails = _mapper.Map<ICollection<MatchDetailResponseDTO>>(collection
                                                .Where(x => x.Id == matchDto.Id)
                                                .FirstOrDefault()
                                                .MatchDetails);
                
                matchDto.MatchDetails.ToList().ForEach(async x =>
                {
                    var resoures = await _repository.Resource.GetResourcesByBird(x.Bird.Id);

                    x.Bird.ResourceList = _mapper.Map<ICollection<ResourceResponse>>(resoures);
                });
            }
            
            return matchDtoList;
        }

        public async Task<PagedList<MatchResponseDTO>> GetMatches(MatchParameters matchParameters)
        {
            _validation.Base.ValidateParameter(matchParameters);

            PagedList<MatchEntity> list = null;

            if (matchParameters.PageSize > 0)
            {
                list = await _repository.Match.GetMatchesWithPaging(matchParameters);
            }

            Console.WriteLine($"Pagination data: {((PagedList<MatchEntity>)list).TotalCount}");
            

            PagedList<MatchResponseDTO> lisDto = _mapper.Map<PagedList<MatchResponseDTO>>(list);

            lisDto.MapMetaData(list);

            foreach (var matchDto in lisDto)
            {
                matchDto.MatchDetails = _mapper.Map<ICollection<MatchDetailResponseDTO>>(list
                                                .Where(x => x.Id == matchDto.Id)
                                                .FirstOrDefault()
                                                .MatchDetails);

                foreach (var resoures in matchDto.MatchDetails)
                {
                    var res = await _repository.Resource.GetResourcesByBird(resoures.Bird.Id);

                    resoures.Bird.ResourceList = _mapper.Map<ICollection<ResourceResponse>>(res);
                }
            }

            return lisDto;
        }

        public async Task UpdateMatch(Guid matchId, MatchUpdateDTO matchUpdateDTO)
        {
            var match = await _repository.Match.GetByIdActiveAsync(matchId);

            if (match == null) throw new BadRequestException("Match not found");

            _mapper.Map(matchUpdateDTO, match);

            await _repository.Match.UpdateAsync(match);
        }

        public async Task JoinMatch(Guid matchId, MatchJoinDTO matchJoinDTO)
        {
            await _validation.Base.ValidateMatchId(matchId);
            await _validation.Base.ValidateAccountId(matchJoinDTO.ChallengerId);

            await _repository.Match.JoinMatch(matchId, matchJoinDTO);
        }

        public async Task ConfirmMatch(Guid matchId, Guid userConfirmId)
        {
            await _validation.Base.ValidateMatchId(matchId);
            await _validation.Base.ValidateAccountId(userConfirmId);

            var match = await _repository.Match.GetByIdActiveAsync(matchId);

            if (match.HostId != userConfirmId)
                throw new BadRequestException("You are not the host of this match");

            if (match.MatchStatus != MatchStatus.Pending)
                throw new BadRequestException("Match is not pending");

            var matchBirds = match.MatchDetails;

            foreach (var matchBird in matchBirds)
            {
                if (matchBird.Result != MatchDetailResult.Ready)
                    throw new BadRequestException("All player must be ready");
            }

            await _repository.Match.ConfirmMatch(matchId);
        }

        public async Task<ICollection<MatchResponseDTO>> GetWithOwnerAndStatus(Guid userId, string rolePlayer, string matchStatus)
        {
            RolePlayer rolePlayerEnum;
            switch (rolePlayer.ToLower())
            {
                case "host":
                    rolePlayerEnum = RolePlayer.Host;
                    break;
                case "challenger":
                    rolePlayerEnum = RolePlayer.Challenger;
                    break;
                default:
                    throw new BadRequestException("Role player not found");
            }

            MatchStatus matchStatusEnum;
            switch (matchStatus.ToLower())
            {
                case "pending":
                    matchStatusEnum = MatchStatus.Pending;
                    break;
                case "during":
                    matchStatusEnum = MatchStatus.During;
                    break;
                case "completed":
                    matchStatusEnum = MatchStatus.Completed;
                    break;
                case "cancelled":
                    matchStatusEnum = MatchStatus.Cancelled;
                    break;
                default:
                    throw new BadRequestException("Match status not found");
            }

            ICollection<MatchEntity> matchList = await _repository.Match.GetWithOwnerAndStatus(userId, rolePlayerEnum, matchStatusEnum);

            var matchDTOList = _mapper.Map<ICollection<MatchResponseDTO>>(matchList);

            foreach(var matchDto in matchDTOList)
            {
                matchDto.MatchDetails.ToList().ForEach(async x =>
                {
                    var resoures = await _repository.Resource.GetResourcesByBird(x.Bird.Id);

                    x.Bird.ResourceList = _mapper.Map<ICollection<ResourceResponse>>(resoures);
                });
            }

            return matchDTOList;
        }

        public async Task<Guid> CreateMatchFromRequest(MatchCreateDTO matchCreateDTO)
        {
            await _validation.Match.ValidateCreateMatch(matchCreateDTO);

            Guid createdId = await _repository.Match.CreateMatchFromRequest(matchCreateDTO);

            return createdId;
        }

        public async Task<PagedList<MatchResponseDTO>> GetMatchByGroupId(Guid groupId, MatchParameters parameters)
        {
            await _validation.Base.ValidateGroupId(groupId);
            _validation.Base.ValidateParameter(parameters);

            PagedList<MatchEntity> matchList = await _repository.Match.GetMatchByGroupId(groupId, parameters);

            var matchDTOList = _mapper.Map<PagedList<MatchResponseDTO>>(matchList);

            foreach(var matchDto in matchDTOList)
            {
                foreach (var resoures in matchDto.MatchDetails)
                {
                    var res = await _repository.Resource.GetResourcesByBird(resoures.Bird.Id);

                    resoures.Bird.ResourceList = _mapper.Map<ICollection<ResourceResponse>>(res);
                }
            }

            return matchDTOList;
        }

        public async Task<ICollection<MatchResponseDTO>> GetMatchesByBirdId(Guid birdId, string matchStatusRaw)
        {
            await _validation.Base.ValidateBirdId(birdId);

            MatchStatus matchStatus;

            ICollection<MatchEntity> matchList;

            if (matchStatusRaw != null)
            {
                var resultParse = Enum.TryParse<MatchStatus>(matchStatusRaw, out matchStatus);

                if (resultParse == false)
                    throw new BadRequestException("Match status not found");

                matchList = await _repository.Match.GetMatchesByBirdId(birdId, matchStatus);
            }
            else
            {
                matchList = await _repository.Match.GetMatchesByBirdId(birdId);
            }

            var matchListDTO = _mapper.Map<ICollection<MatchResponseDTO>>(matchList);

            foreach (var matchDto in matchListDTO)
            {
                matchDto.MatchDetails = _mapper.Map<ICollection<MatchDetailResponseDTO>>(matchList
                                                .Where(x => x.Id == matchDto.Id)
                                                .FirstOrDefault()
                                                .MatchDetails);

                foreach (var resoures in matchDto.MatchDetails)
                {
                    var res = await _repository.Resource.GetResourcesByBird(resoures.Bird.Id);

                    resoures.Bird.ResourceList = _mapper.Map<ICollection<ResourceResponse>>(res);
                }
            }

            return matchListDTO;
        }

        public async Task ResolveMatchResult(Guid userId, Guid matchId, ResolveMatchResultDTO updateData)
        {
            await _validation.Base.ValidateAdmin(userId);
            await _validation.Base.ValidateMatchId(matchId);

            if (updateData.loseBirdId == null || updateData.loseBirdId == null)
            {
                await _repository.Match.ChangeMatchResultToDraw(matchId, updateData);
            }
            else
            {
                await _repository.MatchDetail.UpdateMatchResult(matchId, updateData.winBirdId, "win");
                await _repository.MatchDetail.UpdateMatchResult(matchId, updateData.loseBirdId, "lose");

                var match = await _repository.Match.GetMatch(matchId);

                if (match.MatchStatus == MatchStatus.Completed)
                {
                    bool isInGroup = (match.GroupId != Guid.Empty && match.GroupId != null);

                    await _matchDetailService.UpdateBirdsEloAfterMatchComplete(matchId, isInGroup);
                }
            }
        }
    }
}