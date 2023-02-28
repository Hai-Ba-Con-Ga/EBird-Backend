using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IMatchDetailService
    {
        public Task UpdateBirdInMatch(MatchDetailUpdateDTO matchBirdUpdateDTO);
        Task UpdateChallengerReady(UpdateChallengerToReadyDTO updateData);
        public Task UpdateMatchResult (UpdateMatchResultDTO matchBirdUpdateDTO);
        Task UpdateResultMatch(Guid matchId, MatchDetailUpdateResultDTO updateResultDto, Guid userId);
    }
}