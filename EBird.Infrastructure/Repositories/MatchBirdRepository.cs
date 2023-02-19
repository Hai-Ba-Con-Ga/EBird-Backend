using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EBird.Infrastructure.Repositories
{
    public class MatchBirdRepository : GenericRepository<MatchDetailEntity>, IMatchBirdRepository
    {
        public MatchBirdRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task UpdateMatchBird(UpdateChallengerToReadyDTO updateData)
        {
            var matchBird = await _context.MatchBirds.Where(m => m.MatchId == updateData.MatchId 
                                                        && m.BirdId == updateData.BirdId)
                                                        .FirstOrDefaultAsync();

            if (matchBird == null) throw new Exception("Match Bird not found");

            matchBird.Result = MatchDetailResult.Ready;

            _context.MatchBirds.Update(matchBird);
           await  _context.SaveChangesAsync();
        }

        public async Task UpdateResultMatch(Guid matchId, Guid birdId, string result)
        {
            //  var matchBird = await _repository.MatchBird.FindWithCondition(m => m.MatchId == matchId
            //                                             && m.BirdId == updateResultData.BirdId
            //                                             && m.IsDeleted == false
            //                                             && m.Result == MatchBirdResult.Ready);

            var matchBird = await _context.MatchBirds.Where(m => m.MatchId == matchId
                                                        && m.BirdId == birdId
                                                        && m.IsDeleted == false
                                                        && m.Result == MatchDetailResult.Ready)
                                                        .FirstOrDefaultAsync();

            if (matchBird == null)
            {
                throw new BadRequestException("Match-Bird not found");
            }

            switch (result.ToLower())
            {
                case "win":
                    matchBird.Result = MatchDetailResult.Win;
                    break;
                case "lose":
                    matchBird.Result = MatchDetailResult.Lose;
                    break;
                case "draw":
                    matchBird.Result = MatchDetailResult.Draw;
                    break;
                default:
                    throw new BadRequestException("Result not valid");
            }

            _context.MatchBirds.Update(matchBird);
            await _context.SaveChangesAsync();
        }
    }
}