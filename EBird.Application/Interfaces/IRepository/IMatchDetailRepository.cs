using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IMatchDetailRepository : IGenericRepository<MatchDetailEntity>
    {
        public Task UpdateMatchBird(UpdateChallengerToReadyDTO updateData);
        public Task UpdateResultMatch(Guid matchId, Guid birdId, string result);
    }
}