using EBird.Application.Interfaces.IRepository;
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
    public class BirdTypeRepository : GenericRepository<BirdTypeEntity>, IBirdTypeRepository
    {
        public BirdTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<BirdTypeEntity> SoftDeleteAsync(string birdTypeCode)
        {
            BirdTypeEntity _entity = await GetBirdTypeByCodeAsync(birdTypeCode);

            if(_entity == null)
            {
                return null;
            }
            _entity.IsDeleted = true;

            await this.UpdateAsync(_entity);

            return _entity;
        }

        public async Task<BirdTypeEntity> GetBirdTypeByCodeAsync(string birdTypeCode)
        {
            return await this.FindWithCondition(x => x.TypeCode == birdTypeCode);
        }

        public async Task<bool> IsExistBirdTypeCode(string birdTypeCode)
        {
            var result = await this.FindWithCondition(x => x.TypeCode == birdTypeCode);
            if(result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<BirdTypeEntity>> GetAllBirdTypeActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<BirdTypeEntity> GetBirdTypeActiveAsync(Guid id)
        {
            return await this.GetByIdAsync(id);
        }

        public async Task<BirdTypeEntity> UpdateBirdTypeAsync(BirdTypeEntity birdType)
        {
            var updateEntity = await this.UpdateAsync(birdType);
            if(updateEntity == 0)
            {
                return null;
            }
            return birdType;
        }

        public async Task<BirdTypeEntity> AddBirdTypeAsync(BirdTypeEntity birdType)
        {
            birdType.CreatedDatetime = DateTime.Now;
            var addEntity = await this.CreateAsync(birdType);
            if(addEntity == 0)
            {
                return null;
            }
            return birdType;
        }
    }
}
