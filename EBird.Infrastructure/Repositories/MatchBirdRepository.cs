using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using EBird.Infrastructure.Context;

namespace EBird.Infrastructure.Repositories
{
    public class MatchBirdRepository : GenericRepository<MatchBirdEntity>, IMatchBirdRepository
    {
        public MatchBirdRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task UpdateMatchBird(UpdateChallengerToReadyDTO updateData)
        {
            var matchBird = _context.MatchBirds.Where(m => m.MatchId == updateData.MatchId 
                                                        && m.BirdId == updateData.BirdId)
                                                        .FirstOrDefault();

            if (matchBird == null) throw new Exception("Match Bird not found");

            matchBird.Result = MatchBirdResult.Ready;

            _context.MatchBirds.Update(matchBird);
           await  _context.SaveChangesAsync();
        }
    }
}