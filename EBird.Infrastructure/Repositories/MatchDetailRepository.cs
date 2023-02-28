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
    public class MatchDetailRepository : GenericRepository<MatchDetailEntity>, IMatchDetailRepository
    {
        public MatchDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task UpdateMatchDetail(UpdateChallengerToReadyDTO updateData)
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

            var matchDetail = await _context.MatchBirds.Where(m => m.MatchId == matchId
                                                        && m.BirdId == birdId
                                                        && m.IsDeleted == false
                                                        && m.Result == MatchDetailResult.Ready)
                                                        .FirstOrDefaultAsync();

            if (matchDetail == null)
            {
                throw new BadRequestException("MatchDetail not found");
            }

            switch (result.ToLower())
            {
                case "win":
                    matchDetail.Result = MatchDetailResult.Win;
                    break;
                case "lose":
                    matchDetail.Result = MatchDetailResult.Lose;
                    break;
                case "draw":
                    matchDetail.Result = MatchDetailResult.Draw;
                    break;
                default:
                    throw new BadRequestException("Result not valid");
            }

            _context.MatchBirds.Update(matchDetail);
            await _context.SaveChangesAsync();
        }
    }
}