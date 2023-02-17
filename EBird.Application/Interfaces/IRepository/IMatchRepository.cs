using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IMatchRepository : IGenericRepository<MatchEntity>
    {
        public Task<Guid> CreateMatch(MatchEntity match, MatchBirdEntity matchBirdEntity);
        public Task<MatchEntity> GetMatch(Guid id);
        public Task<ICollection<MatchEntity>> GetMatches(MatchParameters param);
        public Task<ICollection<MatchEntity>> GetMatchesWithPaging(MatchParameters param);
    }
}