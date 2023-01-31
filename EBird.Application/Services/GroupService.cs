using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Services.IServices;
using EBird.Application.Validation;
using EBird.Domain.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EBird.Application.Services
{
    public class GroupService : IGroupService
    {
        private IWapperRepository _repository;
        private IMapper _mapper;

        public GroupService(IWapperRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GroupDTO> AddGroup(GroupDTO groupDTO)
        {
            await GroupValidation.ValidateGroup(groupDTO, _repository);
            
            var groupEntity = _mapper.Map<GroupEntity>(groupDTO);
            
            var resultAction = await _repository.Group.AddGroupAsync(groupEntity);
            
            if(resultAction == null)
            {
                throw new BadRequestException("Group is not added");
            }

            return _mapper.Map<GroupDTO>(resultAction);
        }

        public async Task<GroupDTO> DeleteGroup(Guid groupId)
        {
            var result = await _repository.Group.SoftDeleteGroupAsync(groupId);
            
            if(result is null)
            {
                throw new NotFoundException("Can not found group for delete");
            }
            return _mapper.Map<GroupDTO>(result);
        }

        public async Task<GroupDTO> GetGroup(Guid groupId)
        {
            var birdDTO = await _repository.Group.GetGroupActiveAsync(groupId);
            
            if(birdDTO == null)
            {
                throw new NotFoundException("Can not found a group");
            }
            return _mapper.Map<GroupDTO>(birdDTO);
        }

        public async Task<List<GroupDTO>> GetGroups()
        {
            var birdDTOList = await _repository.Group.GetGroupsActiveAsync();
            
            if(birdDTOList.Count == 0)
            {
                throw new NotFoundException("Can not found Groups");
            }
            return _mapper.Map<List<GroupDTO>>(birdDTOList);
        }

        public async Task<GroupDTO> UpdateGroup(Guid groupId, GroupUpdateDTO groupUpdateDTO)
        {
            await GroupValidation.ValidateGroup(groupUpdateDTO, _repository);
            
            var groupEntity = await _repository.Group.GetGroupActiveAsync(groupId);
            
            if(groupEntity == null)
            {
                throw new NotFoundException("Can not found group for updating");
            }

            _mapper.Map(groupUpdateDTO, groupEntity);

            var result = await _repository.Group.UpdateGroupAsync(groupEntity);
            
            if(result == 0)
            {
                throw new BadRequestException("Update group is fail");
            }

            return _mapper.Map<GroupDTO>(groupEntity);
        }
    }
}
