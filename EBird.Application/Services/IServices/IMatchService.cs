using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Model.Match;

namespace EBird.Application.Services.IServices
{
    public interface IMatchService 
    {
        public Task<Guid> CreateMatch(MatchCreateDTO matchCreateDTO);
        public Task<MatchResponseDTO> GetMatch(Guid matchId);
        public Task<ICollection<MatchResponseDTO>> GetMatches();
        public Task<ICollection<MatchResponseDTO>> GetMatches(MatchParameters matchParameters);
        public Task DeleteMatch(Guid matchId);
        public Task UpdateMatch(Guid matchId, MatchUpdateDTO matchUpdateDTO);
        public Task JoinMatch(Guid matchId, MatchJoinDTO matchJoinDTO);
        Task ConfirmMatch(Guid matchId, Guid userConfirmId);
    }
}