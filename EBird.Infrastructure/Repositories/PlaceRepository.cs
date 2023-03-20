using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;

namespace EBird.Infrastructure.Repositories
{
    public class PlaceRepository : GenericRepository<PlaceEntity>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Guid> CreatePlace(PlaceEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            var result = await this.CreateAsync(entity);
            if (result == 0)
            {
                throw new BadRequestException("Can not create place");
            }
            return entity.Id;
        }

        public async Task<bool> DeletePlace(Guid entityId)
        {
            var result = await this.DeleteAsync(entityId);
            if (result == null)
            {
                throw new BadRequestException("Entity is not exist");
            }
            return true;
        }

        public async Task<PlaceEntity> GetPlace(Guid id)
        {
            var result = await this.GetByIdActiveAsync(id);
            return result;
        }

        public async Task<ICollection<PlaceEntity>> GetPlaces()
        {
            return await this.GetAllAsync();
        }

        public async Task<bool> UpdatePlace(PlaceEntity entity)
        {
            var result = await this.UpdateAsync(entity);
            
            if(result == 0) throw new BadRequestException("Can not update place");
            
            return true;
        }
        public async Task<ICollection<PlaceEntity>> GetFavoritePlaces()
        {
            return await this.FindAllWithCondition(p=> p.FavoritePlace == true);
        }
    }
}