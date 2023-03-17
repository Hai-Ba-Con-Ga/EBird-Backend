using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Match;
using EBird.Application.Model.Resource;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IMatchDetailRepository : IGenericRepository<MatchDetailEntity>
    {
        public Task<IList<MatchDetailEntity>> GetMatchDetailsByMatchId(Guid matchId);
        public Task UpdateMatchDetail(UpdateChallengerToReadyDTO updateData);
        public Task UpdateMatchResult(Guid matchId, Guid birdId, string result, IList<ResourceEntity>? matchResources = null);
        public Task UpdateMatchDetails(IList<MatchDetailEntity> matchDetails);
        public Task<ICollection<ResourceEntity>> GetMatchResources(Guid matchDetailId);
    }
}