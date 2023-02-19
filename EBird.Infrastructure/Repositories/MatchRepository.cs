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

        public async Task<Guid> CreateMatch(MatchEntity match, MatchBirdEntity matchBirdEntity)
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

                    await _context.MatchBirds.AddAsync(matchBirdEntity);
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

        public async Task<MatchEntity> GetMatch(Guid id)
        {
            var entity = await _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchBirds)
                .ThenInclude(e => e.Bird)
                .Where(e => e.Id == id && e.IsDeleted == false)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task<ICollection<MatchEntity>> GetMatchesWithPaging(MatchParameters param)
        {

            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchBirds)
                .ThenInclude(e => e.Bird)
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.CreateDatetime)
                .AsNoTracking();

            if (param.MatchStatus != null)
            {
                collection = collection.Where(e => e.MatchStatus == param.MatchStatus);
            }

            PagedList<MatchEntity> pagedList = new PagedList<MatchEntity>();
            await pagedList.LoadData(collection, param.PageNumber, param.PageSize);

            return pagedList;
        }

        public async Task<ICollection<MatchEntity>> GetMatches(MatchParameters param)
        {
            var collection = _context.Matches
                .Include(e => e.Place)
                .Include(e => e.MatchBirds)
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

                    var matchBird = new MatchBirdEntity();
                    matchBird.MatchId = matchId;
                    matchBird.BirdId = matchJoinDTO.BirdChallengerId;
                    matchBird.Result = MatchDetailResult.NotReady;
                    matchBird.UpdateDatetime = DateTime.Now;

                    var birdEntity = await _context.Birds.FindAsync(matchBird.BirdId);
                    if (birdEntity == null) throw new BadRequestException("Bird not found");

                    matchBird.BeforeElo = birdEntity.Elo;

                    await _context.MatchBirds.AddAsync(matchBird);
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
                .Include(e => e.MatchBirds)
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

       
    }
}