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

        public async Task<IList<MatchDetailEntity>> GetMatchDetailsByMatchId(Guid matchId)
        {
            var matchDetails = await _context.MatchBirds
                                        .Include(mb => mb.Bird)
                                        .Where(mb => mb.MatchId == matchId).ToListAsync();

            return matchDetails;
        }

        public async Task UpdateMatchDetails(IList<MatchDetailEntity> matchDetails)
        {
            _context.MatchBirds.UpdateRange(matchDetails);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMatchDetail(UpdateChallengerToReadyDTO updateData)
        {
            var matchBird = await _context.MatchBirds.Where(m => m.MatchId == updateData.MatchId
                                                        && m.BirdId == updateData.BirdId)
                                                        .FirstOrDefaultAsync();

            if (matchBird == null) throw new Exception("Match Bird not found");

            matchBird.Result = MatchDetailResult.Ready;

            _context.MatchBirds.Update(matchBird);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMatchResult(Guid matchId, Guid birdId, string result)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var matchDetail = await _context.MatchBirds.Where(m => m.MatchId == matchId
                                                           && m.BirdId == birdId
                                                           && m.IsDeleted == false)
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

                    var matchDetailCompetitor = await _context.MatchBirds
                                                        .Where(m => m.MatchId == matchId
                                                           && m.BirdId != birdId
                                                           && m.IsDeleted == false)
                                                           .FirstOrDefaultAsync();

                    if (matchDetailCompetitor == null)
                        throw new BadRequestException("MatchDetail competitor not found");

                    bool isMatchCompleted =
                        (matchDetailCompetitor.Result == matchDetail.Result && matchDetail.Result == MatchDetailResult.Draw)
                        || (matchDetailCompetitor.Result == MatchDetailResult.Win && matchDetail.Result == MatchDetailResult.Lose)
                        || (matchDetailCompetitor.Result == MatchDetailResult.Lose && matchDetail.Result == MatchDetailResult.Win);

                    if (isMatchCompleted)
                    {
                        await UpdateMatchStatus(matchId, MatchStatus.Completed);
                    }
                    else if (matchDetailCompetitor.Result == matchDetail.Result
                        && (matchDetail.Result == MatchDetailResult.Win || matchDetail.Result == MatchDetailResult.Lose))
                    {
                        await UpdateMatchStatus(matchId, MatchStatus.Conflict);
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task UpdateMatchStatus(Guid matchId, MatchStatus matchStatus)
        {
            var match = await _context.Matches
                                            .Where(m => m.Id == matchId)
                                            .FirstOrDefaultAsync();
            if (match == null)
                throw new BadRequestException("Match not found");

            match.MatchStatus = matchStatus;

            _context.Matches.Update(match);

            await _context.SaveChangesAsync();
        }
    }
}