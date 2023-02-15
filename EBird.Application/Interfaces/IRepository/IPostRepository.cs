using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IPostRepository : IGenericRepository<PostEntity>
    {
        Task<List<PostEntity>> GetPostsActiveAsync();
        Task<PostEntity> GetPostActiveAsync(Guid Id);
        Task<Guid> AddPostAsync(PostEntity postEntity);
        Task<Guid> AddPostAsync(PostEntity postEntity, ResourceEntity resourceEntity);
        Task<int> UpdatePostAsync(PostEntity postEntity);
        Task<PostEntity> SoftDeletePostAsync(Guid Id);
    }
}
