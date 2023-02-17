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
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;

namespace EBird.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IWapperRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfValidation _validation;

        public MatchService(IWapperRepository repository, IMapper mapper, IUnitOfValidation validation)
        {
            _repository = repository;
            _mapper = mapper;
            _validation = validation;
        }

        public async Task<Guid> CreateMatch(MatchCreateDTO matchCreateDTO)
        {
            MatchEntity match = _mapper.Map<MatchEntity>(matchCreateDTO);

            MatchBirdEntity matchBird = new MatchBirdEntity()
            {
                BirdId = matchCreateDTO.BirdHostId,
                Result = Domain.Enums.MatchBirdResult.Ready
            };

            var matchId = await _repository.Match.CreateMatch(match, matchBird);

            return matchId;
        }

        public async Task DeleteMatch(Guid matchId)
        {
            await _repository.Match.DeleteSoftAsync(matchId);
        }

        public async Task<MatchResponseDTO> GetMatch(Guid matchId)
        {
            var match = await _repository.Match.GetMatch(matchId);

            if (match == null) throw new BadRequestException("Match not found");

            var matchDTO = _mapper.Map<MatchResponseDTO>(match);
            matchDTO.MatchBirdList = _mapper.Map<ICollection<MatchBirdResponseDTO>>(match.MatchBirds);

            return matchDTO;
        }

        public async Task<ICollection<MatchResponseDTO>> GetMatches()
        {
            var collection = await _repository.Match.GetAllActiveAsync();

            return _mapper.Map<ICollection<MatchResponseDTO>>(collection);
        }

        public async Task<ICollection<MatchResponseDTO>> GetMatches(MatchParameters matchParameters)
        {
            _validation.Base.ValidateParameter(matchParameters);

            ICollection<MatchEntity> list = null;

            if (matchParameters.PageSize > 0)
            {
                list = await _repository.Match.GetMatchesWithPaging(matchParameters);
            }
            else
            {
                list = await _repository.Match.GetMatches(matchParameters);
            }

            ICollection<MatchResponseDTO> lisDto =  _mapper.Map<ICollection<MatchResponseDTO>>(list);
            
            foreach (var item in lisDto)
            {
                item.MatchBirdList = _mapper.Map<ICollection<MatchBirdResponseDTO>>(list
                                                .Where(x => x.Id == item.Id)
                                                .FirstOrDefault()
                                                .MatchBirds);
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
    }
}