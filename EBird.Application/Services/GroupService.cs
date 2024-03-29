﻿using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Group;
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

        private IUnitOfValidation _unitOfValidation;

        public GroupService(IWapperRepository repository, IMapper mapper, IUnitOfValidation unitOfValidation)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfValidation = unitOfValidation;
        }

        public async Task<Guid> AddGroup(GroupCreateDTO groupDTO)
        {
            _unitOfValidation.Group.ValidateGroup(groupDTO);

            var groupEntity = _mapper.Map<GroupEntity>(groupDTO);

            await _repository.Group.AddGroupAsync(groupEntity);

            return groupEntity.Id;
        }

        public async Task DeleteGroup(Guid groupId)
        {
            await _repository.Group.SoftDeleteGroupAsync(groupId);
        }

        public async Task<GroupResponseDTO> GetGroup(Guid groupId)
        {
            var birdDTO = await _repository.Group.GetGroupActiveAsync(groupId);

            if(birdDTO == null)
            {
                throw new NotFoundException("Can not found a group");
            }
            return _mapper.Map<GroupResponseDTO>(birdDTO);
        }

        public async Task<List<GroupResponseDTO>> GetGroups()
        {
            var birdDTOList = await _repository.Group.GetGroupsActiveAsync();
            
            return _mapper.Map<List<GroupResponseDTO>>(birdDTOList);
        }

        public async Task UpdateGroup(Guid groupId, GroupRequestDTO groupUpdateDTO)
        {
            _unitOfValidation.Group.ValidateGroup(groupUpdateDTO);

            var groupEntity = await _repository.Group.GetGroupActiveAsync(groupId);

            if(groupEntity == null)
            {
                throw new NotFoundException("Can not found group for updating");
            }

            _mapper.Map(groupUpdateDTO, groupEntity);

            await _repository.Group.UpdateGroupAsync(groupEntity);
        }
    }
}
