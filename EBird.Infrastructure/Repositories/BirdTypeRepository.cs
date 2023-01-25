using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
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

        public async Task<BirdTypeEntity> DeleteSoftAsync(string birdTypeCode)
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

        public bool IsExistBirdTypeCode(string birdTypeCode)
        {
            return this.FindWithCondition(x => x.TypeCode == birdTypeCode).Result != null;
        }
    }

}
