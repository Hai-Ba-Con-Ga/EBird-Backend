using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IPlaceRepository
    {
        public Task<bool> CreatePlace(PlaceEntity entity);
        public Task<bool> DeletePlace(Guid entityId);
        public Task<bool> UpdatePlace(PlaceEntity entity);
        public Task<PlaceEntity> GetPlace(Guid id); 
        public Task<ICollection<PlaceEntity>> GetPlaces(); 
    }
}