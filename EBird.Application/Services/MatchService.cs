using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Match;
using EBird.Application.Services.IServices;

namespace EBird.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IWapperRepository _repository;
        private readonly IMapper _mapper;

        public MatchService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateMatch(MatchCreateDTO matchCreateDTO)
        {
           var matchId = await _repository.Match
                        .CreateMatch(matchCreateDTO.RequestId, 
                                    matchCreateDTO.ChallengerId, 
                                    matchCreateDTO.ChallangerBirdId);
           return matchId;
        }

        public async Task DeleteMatch(Guid matchId)
        {
            await _repository.Match.DeleteSoftAsync(matchId);
        }

        public async Task<MatchResponseDTO> GetMatch(Guid matchId)
        {
            var match = await _repository.Match.GetByIdActiveAsync(matchId);

            if(match == null) throw new BadRequestException("Match not found");

            var matchDTO = _mapper.Map<MatchResponseDTO>(match);

            return matchDTO;
        }

        public async Task<ICollection<MatchResponseDTO>> GetMatches()
        {
            var collection = await _repository.Match.GetAllActiveAsync();

            return _mapper.Map<ICollection<MatchResponseDTO>>(collection);
        }

        public async Task UpdateMatch(Guid matchId, MatchUpdateDTO matchUpdateDTO)
        {
            var match = await _repository.Match.GetByIdActiveAsync(matchId);

            if(match == null) throw new BadRequestException("Match not found");

            _mapper.Map(matchUpdateDTO, match);

            await _repository.Match.UpdateAsync(match);
        }
    }
}