using EBird.Application.Model.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IPostService
    {
        public Task<Guid> AddPost(PostRequestWithIdAccountDTO ntDTO);
        public Task UpdatePost(Guid id, PostRequestDTO ntDTO);
        public Task DeletePost(Guid id);
        public Task<PostResponseDTO> GetPost(Guid id);
        public Task<List<PostResponseDTO>> GetPosts();
    }
}
