using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model;
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

        public async Task<BirdEntity> AddBirdAsync(BirdEntity bird)
        {
            bird.CreatedDatetime = DateTime.Now;
            int rowEffect = await this.CreateAsync(bird);
            if(rowEffect > 0)
            {
                return bird;
            }
            return null;
        }

        public async Task<List<BirdEntity>> GetAllBirdActiveByAccountId(Guid accountId)
        {
            return await this.FindAllWithCondition(b => b.OwnerId.Equals(accountId) 
                                                        && b.IsDeleted == false);
        }

        public async Task<BirdEntity> GetBirdActiveAsync(Guid birdID)
        {
            return await this.GetByIdActiveAsync(birdID);
        }

        public async Task<List<BirdEntity>> GetBirdsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        /// <summary>
        /// Get bird list within paging
        /// </summary>
        /// <param name="birdParameters"></param>
        /// <returns></returns>
        public async Task<List<BirdEntity>> GetBirdsActiveAsync(BirdParameters birdParameters)
        {
            var birds =  _context.Set<BirdEntity>().AsNoTracking();
            
            birds = birds.OrderBy(b => b.Elo);

            return await PagedList<BirdEntity>.ToPageList(birds, birdParameters.PageNumber, birdParameters.PageSize);
        }

        public async Task<BirdEntity> SoftDeleteBirdAsync(Guid birdID)
        {
            return await this.DeleteSoftAsync(birdID);
        }

        public async Task<BirdEntity> UpdateBirdAsync(BirdEntity bird)
        {
            int rowEffect = await this.UpdateAsync(bird);
            if(rowEffect == 0)
            {
                return null;
            }
            return bird;
        }
    }
}
