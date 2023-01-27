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
            BirdTypeEntity _entity = await GetBirdTypeByCode(birdTypeCode);
            if(_entity == null)
            {
                return null;
            }

            _entity.IsDeleted = true;
            await this.UpdateAsync(_entity);
            return _entity;
        }

        public async Task<BirdTypeEntity> GetBirdTypeByCode(string birdTypeCode)
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
            return await this.FindAllWithCondition(x => x.IsDeleted == false);
        }

        public async Task<BirdTypeEntity> GetBirdTypeActiveAsync(Guid id)
        {
            return await this.FindWithCondition(x => x.Id == id && x.IsDeleted == false);
        }
    }
}
    