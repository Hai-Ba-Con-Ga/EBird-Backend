using EBird.Application.Exceptions;
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
    public class PostRepository : GenericRepository<PostEntity>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PostEntity> SoftDeletePostAsync(Guid Id)
        {
            return await DeleteSoftAsync(Id);
        }

        public async Task<List<PostEntity>> GetPostsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<PostEntity> GetPostActiveAsync(Guid id)
        {
            return await this.GetByIdActiveAsync(id);
        }

        public async Task<int> UpdatePostAsync(PostEntity postEntity)
        {
            return await this.UpdateAsync(postEntity);
        }

        public async Task<Guid> AddPostAsync(PostEntity postEntity)
        {
            var addEntity = await this.CreateAsync(postEntity);
            if (addEntity == 0)
            {
                throw new BadRequestException("can not create post entity");
            }
            return postEntity.Id;
        }
        public async Task<Guid> AddPostAsync(PostEntity post, ResourceEntity resource)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    int rowEffect;

                    post.CreateDateTime = DateTime.Now;
                    post.ThumbnailId = resource.Id;
                    await _context.Posts.AddAsync(post);
                    rowEffect = await _context.SaveChangesAsync();

                    if (rowEffect == 0) throw new BadRequestException("Post can not be added");

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                return post.Id;
            }
        }
    }
}
