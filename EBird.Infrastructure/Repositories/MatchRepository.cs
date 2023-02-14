using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using EBird.Infrastructure.Context;

namespace EBird.Infrastructure.Repositories
{
    public class MatchRepository : GenericRepository<MatchEntity>, IMatchRepository
    {
        public MatchRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Guid> CreateMatch(Guid requestId, Guid challengerId, Guid challangerBirdId)
        {
            using (var transction = _context.Database.BeginTransaction())
            {
                try
                {
                    var request = await _context.Requests.FindAsync(requestId);

                    if(request == null) throw new BadRequestException("Request not found");

                    var match = new MatchEntity()
                    {
                        MatchDatetime = request.RequestDatetime,
                        CreateDatetime = DateTime.Now,
                        MatchStatus = MatchStatus.Pending,
                        HostId = request.CreatedById,
                        ChallengerId = challengerId,
                        PlaceId = request.PlaceId
                    };

                    await _context.Matches.AddAsync(match);

                    await _context.SaveChangesAsync();

                    var birds = new List<Guid>();
                    birds.Add(request.BirdId);
                    birds.Add(challangerBirdId);

                    foreach (var birdId in birds)
                    {
                        var currentBird = await _context.Birds.FindAsync(birdId);

                        if (currentBird == null) throw new BadRequestException("Bird not found");

                        var matchBird = new MatchBirdEntity()
                        {
                            MatchId = match.Id,
                            BirdId = birdId,
                            BeforeElo = currentBird.Elo,
                            UpdateDatetime = DateTime.Now
                        };

                        await _context.MatchBirds.AddAsync(matchBird);

                        await _context.SaveChangesAsync();
                    }
                   
                    transction.Commit();

                    return match.Id;
                }
                catch (Exception ex)
                {
                    transction.Rollback();
                    throw ex;
                }
            }
        }
    }
}