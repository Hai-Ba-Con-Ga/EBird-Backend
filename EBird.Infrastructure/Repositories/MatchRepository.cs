using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model.Match;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EBird.Infrastructure.Repositories
{
    public class MatchRepository : GenericRepository<MatchEntity>, IMatchRepository
    {
        public MatchRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Guid> CreateMatch(MatchEntity match, MatchDetailEntity matchBirdEntity)
        {
            using (var transction = _context.Database.BeginTransaction())
            {
                try
                {
                    match.MatchStatus = MatchStatus.Pending;
                    match.CreateDatetime = DateTime.Now;
                    match.ExpDatetime = match.CreateDatetime.AddHours(48);

                    var place = match.Place;
                    if (place != null) place.CreatedDate = DateTime.Now;

                    await _context.Matches.AddAsync(match);
                    var rowEffect = await _context.SaveChangesAsync();

                    matchBirdEntity.MatchId = match.Id;
                    matchBirdEntity.UpdateDatetime = DateTime.Now;
                    matchBirdEntity.Result = MatchDetailResult.Ready;

                    var birdEntity = await _context.Birds.FindAsync(matchBirdEntity.BirdId);
                    if (birdEntity == null) throw new BadRequestException("Bird not found");
                    matchBirdEntity.BeforeElo = birdEntity.Elo;

                    await _context.MatchDetails.AddAsync(matchBirdEntity);
                    await _context.SaveChangesAsync();

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

        public async Task<MatchEntity?> GetMatch(Guid id)
        {
            var entity = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchDetails)
                .ThenInclude(e => e.Bird);
                
            return await entity.FirstOrDefaultAsync(e => e.Id == id && e.IsDeleted == false);
        }

        public async Task<PagedList<MatchEntity>> GetMatchesWithPaging(MatchParameters param)
        {

            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchDetails)
                .ThenInclude(e => e.Bird)
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreateDatetime)
                .AsNoTracking();

            if (param.MatchStatus != null)
            {
                collection = collection.Where(e => e.MatchStatus == param.MatchStatus);
            }

            PagedList<MatchEntity> pagedList = new PagedList<MatchEntity>();

            if (param.PageSize != 0)
            {
                await pagedList.LoadData(collection, param.PageNumber, param.PageSize);
            }
            else
            {
                await pagedList.LoadData(collection);
            }

            return pagedList;
        }

        public async Task<ICollection<MatchEntity>> GetMatches(MatchParameters param)
        {
            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchDetails)
                .ThenInclude(e => e.Bird)
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreateDatetime)
                .AsNoTracking();

            if (param.MatchStatus != null)
            {
                collection = collection.Where(e => e.MatchStatus == param.MatchStatus);
            }

            return await collection.ToListAsync();
        }

        public async Task<ICollection<MatchEntity>> GetMatches()
        {
            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchDetails)
                .ThenInclude(e => e.Bird)
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreateDatetime)
                .AsNoTracking();

            return await collection.ToListAsync();
        }

        public async Task JoinMatch(Guid matchId, MatchJoinDTO matchJoinDTO)
        {
            var match = await _context.Matches.FindAsync(matchId);

            if (match == null) throw new BadRequestException("Match not found");

            var matchStatus = match.MatchStatus;

            match.MatchStatus = MatchStatus.Pending;
            match.ChallengerId = matchJoinDTO.ChallengerId;

            using (var transction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Matches.Update(match);
                    await _context.SaveChangesAsync();

                    var matchBird = new MatchDetailEntity();
                    matchBird.MatchId = matchId;
                    matchBird.BirdId = matchJoinDTO.BirdChallengerId;
                    matchBird.Result = MatchDetailResult.NotReady;
                    matchBird.UpdateDatetime = DateTime.Now;

                    var birdEntity = await _context.Birds.FindAsync(matchBird.BirdId);
                    if (birdEntity == null) throw new BadRequestException("Bird not found");

                    matchBird.BeforeElo = birdEntity.Elo;

                    await _context.MatchDetails.AddAsync(matchBird);
                    await _context.SaveChangesAsync();

                    transction.Commit();
                }
                catch (Exception ex)
                {
                    transction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task ConfirmMatch(Guid matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);

            if (match == null) throw new BadRequestException("Match not found");

            match.MatchStatus = MatchStatus.During;
            match.ExpDatetime = DateTime.Now.AddHours(48);

            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<MatchEntity>> GetWithOwnerAndStatus(Guid userId, RolePlayer rolePlayer, MatchStatus matchStatus)
        {
            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchDetails)
                .ThenInclude(e => e.Bird)
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreateDatetime)
                .AsNoTracking();

            if (rolePlayer == RolePlayer.Challenger)
            {
                collection = collection.Where(e => e.ChallengerId == userId);
            }
            else if (rolePlayer == RolePlayer.Host)
            {
                collection = collection.Where(e => e.HostId == userId);
            }

            if (matchStatus != null)
            {
                collection = collection.Where(e => e.MatchStatus == matchStatus);
            }

            return await collection.ToListAsync();
        }

        public async Task<Guid> CreateMatchFromRequest(MatchCreateDTO matchCreateDTO)
        {
            var request = await _context.Requests.FindAsync(matchCreateDTO.RequestId);

            if (request == null) throw new BadRequestException("Request not found");

            using (var transction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Create match
                    MatchEntity match = new MatchEntity()
                    {
                        HostId = request.HostId,
                        ChallengerId = request.ChallengerId,
                        GroupId = request.GroupId,
                        PlaceId = request.PlaceId,
                        RoomId = request.RoomId,
                        MatchDatetime = request.RequestDatetime,
                        MatchStatus = MatchStatus.During,
                        CreateDatetime = DateTime.Now,
                        ExpDatetime = DateTime.Now.AddDays(2),
                        FromRequestId = request.Id,

                    };

                    await _context.Matches.AddAsync(match);
                    var rowEffect = await _context.SaveChangesAsync();


                    //Create match details
                    Dictionary<string, Guid> guidDictionary = new Dictionary<string, Guid>()
                    {
                          {"host", request.HostBirdId},
                          {"challenger", request.ChallengerBirdId ?? Guid.Empty}
                    };

                    //Create match detail for each player (host and challenger)
                    foreach (var item in guidDictionary)
                    {
                        if (item.Value == Guid.Empty)
                            throw new BadRequestException("Bird is empty");

                        var matchDetail = new MatchDetailEntity()
                        {
                            MatchId = match.Id,
                            BirdId = item.Value,
                            Result = MatchDetailResult.NotReady,
                            UpdateDatetime = DateTime.Now
                        };

                        if (item.Key.Equals("host"))
                        {
                            matchDetail.Result = MatchDetailResult.Ready;
                        }

                        var birdEntity = await _context.Birds.FindAsync(matchDetail.BirdId);

                        if (birdEntity == null) throw new BadRequestException("Bird not found");

                        matchDetail.BeforeElo = birdEntity.Elo;

                        await _context.MatchDetails.AddAsync(matchDetail);
                        await _context.SaveChangesAsync();

                        // birdEntity.Status = BirdStatus.InMatch.GetDescription();
                        // _context.Birds.Update(birdEntity);
                        // await _context.SaveChangesAsync();
                    }

                    //Enable request : change request status to closed state
                    request.Status = RequestStatus.Closed;
                    _context.Requests.Update(request);
                    await _context.SaveChangesAsync();

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
        public async Task<ICollection<MatchEntity>> GetMatchByGroupId(Guid groupId)
        {
            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchDetails)
                .ThenInclude(e => e.Bird)
                .Where(e => e.IsDeleted == false);

            if (groupId != null)
            {
                collection = collection.Where(e => e.GroupId == groupId);
            }

            return await collection
                        .OrderByDescending(e => e.CreateDatetime)
                        .ToListAsync();
        }

        public async Task<ICollection<MatchEntity>> GetMatchesByBirdId(Guid birdId, MatchStatus matchStatus)
        {
            var result = _context.Matches
                            .OrderByDescending(m => m.CreateDatetime)
                            .Include(m => m.MatchDetails)
                            .ThenInclude(md => md.Bird)
                            .Include(m => m.Place)
                            .Where(m => m.MatchStatus == matchStatus
                                    && m.MatchDetails.Any(md => md.BirdId == birdId));

            return await result.ToListAsync();
        }

        public async Task<ICollection<MatchEntity>> GetMatchesByBirdId(Guid birdId)
        {
            var result = _context.Matches
                            .OrderByDescending(m => m.CreateDatetime)
                            .Include(m => m.MatchDetails)
                            .ThenInclude(md => md.Bird)
                            .Include(m => m.Place)
                            .Where(m => m.MatchDetails.Any(md => md.BirdId == birdId));

            return await result.ToListAsync();
        }

        public async Task ChangeMatchResultToDraw(Guid matchId, ResolveMatchResultDTO updateData)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var match = await _context.Matches.FindAsync(matchId);

                    match.MatchStatus = MatchStatus.Approved;

                    _context.Matches.Update(match);

                    await _context.SaveChangesAsync();

                    var matchDetails = await _context.MatchDetails.Where(e => e.MatchId == matchId).ToListAsync();

                    foreach (var matchDetail in matchDetails)
                    {
                        matchDetail.Result = MatchDetailResult.Draw;
                        matchDetail.UpdateDatetime = DateTime.Now;
                        matchDetail.AfterElo = matchDetail.BeforeElo;
                    }

                    _context.UpdateRange(matchDetails);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }
    }
}