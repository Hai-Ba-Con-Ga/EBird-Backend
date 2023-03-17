using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    internal class BirdRepository : GenericRepository<BirdEntity>, IBirdRepository
    {
        public BirdRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> AddBirdAsync(BirdEntity bird)
        {
            bird.CreatedDatetime = DateTime.Now;

            int rowEffect = await this.CreateAsync(bird);

            if (rowEffect == 0)
            {
                throw new BadRequestException("Bird is not created");
            }

            return true;
        }

        public async Task<bool> AddBirdAsync(BirdEntity bird, List<ResourceEntity> resourceList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bird.CreatedDatetime = DateTime.Now;
                    await _context.Birds.AddAsync(bird);
                    int rowEffect = await _context.SaveChangesAsync();

                    if (rowEffect == 0) throw new BadRequestException("Bird is can be added");

                    Guid birdId = bird.Id;

                    foreach (var rsrc in resourceList)
                    {
                        rsrc.CreateDate = DateTime.Now;
                        await _context.Resources.AddAsync(rsrc);
                        rowEffect = await _context.SaveChangesAsync();

                        if (rowEffect == 0) throw new BadRequestException("Resource is can be added");

                        await _context.BirdResources.AddAsync(new BirdResourceEntity(birdId, rsrc.Id));
                        rowEffect = await _context.SaveChangesAsync();

                        if (rowEffect == 0) throw new BadRequestException("Bird_Resource is can be added");
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    throw exception;
                }
                return true;
            }
        }

        public async Task<List<BirdEntity>> GetAllBirdActiveByAccountId(Guid accountId)
        {
            var birdsQuey = dbSet
                            .Include(b => b.Owner)
                            .Include(b => b.BirdType)
                            .Where(b => b.OwnerId.Equals(accountId) && b.IsDeleted == false)
                            .OrderByDescending(b => b.CreatedDatetime)
                            .AsNoTracking();

            return await birdsQuey.ToListAsync();
        }

        public async Task<BirdEntity> GetBirdActiveAsync(Guid birdID)
        {
            var birdQuery = dbSet
                            .Include(b => b.Owner)
                            .Include(b => b.BirdType)
                            .Where(b => b.Id.Equals(birdID) && b.IsDeleted == false)
                            .AsNoTracking();
            return await birdQuery.FirstOrDefaultAsync();
        }

        public async Task<List<BirdEntity>> GetBirdsActiveAsync()
        {
            var birds = dbSet
                        .Include(b => b.Owner)
                        .Include(b => b.BirdType)
                        .AsNoTracking()
                        .OrderByDescending(b => b.Elo);
            return await birds.ToListAsync();
        }

        /// <summary>
        /// Get bird list within paging
        /// </summary>
        /// <param name="birdParameters"></param>
        /// <returns></returns>
        public async Task<PagedList<BirdEntity>> GetBirdsActiveAsync(BirdParameters birdParameters)
        {
            PagedList<BirdEntity> pagedList = new PagedList<BirdEntity>();

            var birds = dbSet
                        .Include(b => b.Owner)
                        .Include(b => b.BirdType)
                        .OrderByDescending(b => b.CreatedDatetime)
                        .AsNoTracking();

            if (birdParameters.SortElo.ToLower().Trim() == "desc")
            {
                birds = birds.OrderByDescending(b => b.Elo);
            }
            else if (birdParameters.SortElo.ToLower().Trim() == "asc")
            {
                birds = birds.OrderBy(b => b.Elo);
            }

            if (birdParameters.PageSize == 0)
            {
                await pagedList.LoadData(birds);
            }
            else
            {
                await pagedList.LoadData(birds, birdParameters.PageNumber, birdParameters.PageSize);
            }

            return pagedList;
        }

        public async Task<long> GetBirdRank(Guid birdId)
        {
            long ranking = 0;

            string sql = @$"SELECT Rank
                            FROM (SELECT * , RANK() OVER (ORDER BY BirdElo DESC) as Rank FROM Bird) as br
                            WhERE Id = '{birdId}' ";

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;

                _context.Database.OpenConnection();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        ranking = reader.GetInt64(0);
                    }
                }
            }

            return ranking;
        }

        public async Task<bool> SoftDeleteBirdAsync(Guid birdID)
        {
            var result = await this.DeleteSoftAsync(birdID);
            if (result == null)
            {
                throw new BadRequestException("Bird is not deleted");
            }
            return true;
        }

        public async Task<bool> UpdateBirdAsync(BirdEntity bird)
        {
            int rowEffect = await this.UpdateAsync(bird);
            if (rowEffect == 0)
            {
                throw new BadRequestException("Can not update bird");
            }
            return true;
        }
    }
}
