using EBird.Application.Exceptions;
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

        public async Task<bool> SoftDeleteAsync(string birdTypeCode)
        {
            BirdTypeEntity _entity = await GetBirdTypeByCodeAsync(birdTypeCode);

            if(_entity == null)
            {
                throw new BadRequestException("Not found bird type for delete");
            }

            _entity.IsDeleted = true;

            int rowEffect = await this.UpdateAsync(_entity);

            if(rowEffect == 0)
            {
                throw new BadRequestException("Can not delete bird type");
            }

            return true;
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

        public async Task<bool> UpdateBirdTypeAsync(BirdTypeEntity birdType)
        {
            var rowEffect = await this.UpdateAsync(birdType);

            if(rowEffect == 0)
            {
                throw new  BadRequestException("Can not update bird type");
            }
            
            return true;
        }

        public async Task<bool> AddBirdTypeAsync(BirdTypeEntity birdType)
        {
            birdType.CreatedDatetime = DateTime.Now;
            
            int rowEffect = await this.CreateAsync(birdType);
            
            if(rowEffect == 0)
            {
                throw new BadRequestException("Can not add bird type");
            }
            
            return true;
        }
    }
}
