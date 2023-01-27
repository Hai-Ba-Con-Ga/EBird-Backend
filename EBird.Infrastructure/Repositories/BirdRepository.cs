using EBird.Application.Interfaces.IRepository;
using EBird.Application.Model;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
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

        public async Task<BirdEntity> GetBirdActiveAsync(Guid birdID)
        {
            return await this.GetByIdActiveAsync(birdID);
        }

        public Task<List<BirdEntity>> GetBirdsActiveAsync()
        {
            return this.GetAllActiveAsync();
        }

        public Task<BirdEntity> SoftDeleteBirdAsync(Guid birdID)
        {
            return this.DeleteSoftAsync(birdID);
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
