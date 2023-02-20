using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Post;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class PostService : IPostService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;
        private IUnitOfValidation _unitOfValidation;

        public PostService(IWapperRepository repository, IMapper mapper, IUnitOfValidation unitOfValidation)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfValidation = unitOfValidation;
        }

        public async Task<Guid> AddPost(PostRequestWithIdAccountDTO ntDTO)
        {
            // check validation
            await _unitOfValidation.Bird.ValidateAccountId(ntDTO.CreateById);
            // convert DTO to Entity
            var entity = _mapper.Map<PostEntity>(ntDTO);
            
            if (ntDTO.Thumbnail == null)
                return await _repository.Post.AddPostAsync(entity);

            //add post with resource.
            var resourceEntity = _mapper.Map<ResourceEntity>(ntDTO.Thumbnail);
            return await _repository.Post.AddPostAsync(entity, resourceEntity);

        }

        public async Task UpdatePost(Guid Id, PostRequestDTO ntDTO)
        {
            // check validation
            //await PostValidation.ValidationPost(ntDTO, _repository);
            
            var entity = await _repository.Post.GetPostActiveAsync(Id);
            if (entity == null)
                throw new NotFoundException("Can not found Post for updating");

            _mapper.Map(ntDTO, entity);
            await _repository.Post.UpdatePostAsync(entity);
        }

        public async Task DeletePost(Guid id)
        {
            await _repository.Post.DeleteSoftAsync(id);
        }
        public async Task<List<PostResponseDTO>> GetPosts()
        {
            var result = await _repository.Post.GetPostsActiveAsync();
            if (result == null || result.Count == 0)
                throw new NotFoundException("Post not found");
            foreach(var item in result)
            {
                if (item.ThumbnailId == null) continue;
                item.Thumbnail = await _repository.Resource.GetResource((Guid)item.ThumbnailId);
            }
            return _mapper.Map<List<PostResponseDTO>>(result);
        }

        public async Task<PostResponseDTO> GetPost(Guid Id)
        {
            var result = await _repository.Post.GetPostActiveAsync(Id);
            if (result == null)
                throw new NotFoundException("Post not found");
            if (result.ThumbnailId != null) 
                result.Thumbnail = await _repository.Resource.GetResource((Guid)result.ThumbnailId);
            return _mapper.Map<PostResponseDTO>(result);
        }


    }
}
