using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Model.Match;
using EBird.Domain.Enums;

namespace EBird.Application.Services.IServices
{
    public interface IMatchService
    {
        // public Task<Guid> CreateMatch(MatchCreateDTO matchCreateDTO);
        public Task<MatchResponseDTO> GetMatch(Guid matchId);
        public Task<ICollection<MatchResponseDTO>> GetMatches();
        public Task<ICollection<MatchResponseDTO>> GetMatches(MatchParameters matchParameters);
        public Task DeleteMatch(Guid matchId);
        public Task UpdateMatch(Guid matchId, MatchUpdateDTO matchUpdateDTO);
        public Task JoinMatch(Guid matchId, MatchJoinDTO matchJoinDTO);
        Task ConfirmMatch(Guid matchId, Guid userConfirmId);
        public Task<ICollection<MatchResponseDTO>> GetWithOwnerAndStatus(Guid userId, string rolePlayer, string matchStatus);
        Task<Guid> CreateMatchFromRequest(MatchCreateDTO matchCreateDTO);
    }
}