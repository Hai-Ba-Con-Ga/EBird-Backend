using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IMatchRepository : IGenericRepository<MatchEntity>
    {
        public Task ConfirmMatch(Guid matchId);
        public Task<Guid> CreateMatch(MatchEntity match, MatchDetailEntity matchBirdEntity);
        Task<Guid> CreateMatchFromRequest(MatchCreateDTO matchCreateDTO);
        public Task<MatchEntity> GetMatch(Guid id);
        public Task<ICollection<MatchEntity>> GetMatches();
        public Task<ICollection<MatchEntity>> GetMatches(MatchParameters param);
        public Task<ICollection<MatchEntity>> GetMatchesWithPaging(MatchParameters param);
        public Task<ICollection<MatchEntity>> GetWithOwnerAndStatus(Guid userId, RolePlayer rolePlayer, MatchStatus matchStatus);
        public Task JoinMatch(Guid matchId, MatchJoinDTO matchJoinDTO);
        public Task<ICollection<MatchEntity>> GetMatchByGroupId(Guid groupId);
    }
}