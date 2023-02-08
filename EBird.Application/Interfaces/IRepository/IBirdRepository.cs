using EBird.Application.Model;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IBirdRepository : IGenericRepository<BirdEntity>
    {
        Task<List<BirdEntity>> GetBirdsActiveAsync();
        Task<BirdEntity> GetBirdActiveAsync(Guid birdID);
        Task<bool> AddBirdAsync(BirdEntity bird);
        Task<bool> UpdateBirdAsync(BirdEntity bird);
        Task<bool> SoftDeleteBirdAsync(Guid birdID);
        Task<PagedList<BirdEntity>> GetBirdsActiveAsync(BirdParameters birdParameters);
        Task<List<BirdEntity>> GetAllBirdActiveByAccountId(Guid accountId);
        
    }
}
