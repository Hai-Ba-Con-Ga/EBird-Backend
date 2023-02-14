using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IMatchRepository : IGenericRepository<MatchEntity>
    {
        public Task<Guid> CreateMatch(Guid requestId, Guid challengerId, Guid challangerBirdId);
    }
}