using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.GroupMember;
using EBird.Application.Services.IServices;
using EBird.Domain.Entities;

namespace EBird.Application.Services
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IMapper _mapper;
        private readonly IWapperRepository _repository;
        private readonly IUnitOfValidation _validation;

        public GroupMemberService(IMapper mapper, IWapperRepository wapperRepository, IUnitOfValidation validation)
        {
            _mapper = mapper;
            _repository = wapperRepository;
            _validation = validation;
        }

        public async Task<ICollection<GroupMemberResponseDTO>> GetGroupsHaveJoined(Guid userId)
        {
            await _validation.Base.ValidateAccountId(userId);

            var groupMembers = await _repository.GroupMember.GetGroupsHaveJoined(userId);   

            return _mapper.Map<ICollection<GroupMemberResponseDTO>>(groupMembers);
        }

        public async Task<Guid> JoinGroup(Guid userId, Guid groupId)
        {
            await _validation.GroupMember.ValidateJoinGroup(userId, groupId);

            GroupMemberEntity groupMember = new GroupMemberEntity
            {
                GroupId = groupId,
                UserId = userId,
                JoinDate = DateTime.Now
            };

            await _repository.GroupMember.CreateAsync(groupMember);

            return groupMember.Id;
        }

        public async Task OutGroup(Guid userId, Guid groupId)
        {
            await _validation.GroupMember.ValidateOutGroup(userId, groupId);

            await _repository.GroupMember.OutGroup(userId, groupId);
        }
    }
}