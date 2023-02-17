using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IMatchBirdRepository : IGenericRepository<MatchBirdEntity>
    {
        public Task UpdateMatchBird(UpdateChallengerToReadyDTO updateData);
    }
}