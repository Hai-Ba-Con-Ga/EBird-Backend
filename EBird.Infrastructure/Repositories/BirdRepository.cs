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
            
            if(rowEffect == 0)
            {
                throw new BadRequestException("Bird is not created");
            }
            
            return true;
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
        public async Task<PagedList<BirdEntity>> GetBirdsActiveAsync(BirdParameters birdParameters)
        {
            var birds = _context.Set<BirdEntity>().AsNoTracking();
            birds = birds.OrderByDescending(b => b.Elo);

            PagedList<BirdEntity> pagedList = new PagedList<BirdEntity>();
            await pagedList.LoadData(birds, birdParameters.PageNumber, birdParameters.PageSize);

            return pagedList;
        }

        public async Task<bool> SoftDeleteBirdAsync(Guid birdID)
        {
            var result =  await this.DeleteSoftAsync(birdID);
            if(result == null)
            {
                throw new BadRequestException("Bird is not deleted");
            }
            return true;
        }

        public async Task<bool> UpdateBirdAsync(BirdEntity bird)
        {
            int rowEffect = await this.UpdateAsync(bird);
            if(rowEffect == 0)
            {
                throw new BadRequestException("Can not update bird");
            }
            return true;
            ;
        }
    }
}
