using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using Microsoft.VisualBasic;

namespace EBird.Application.Validation
{
    public class GroupMemberValidation : BaseValidation, IGroupMemberValidation
    {
        public GroupMemberValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateJoinGroup(Guid userId, Guid groupId)
        {
            bool isVipAccount = await _repository.Account.IsVipAccount(userId);

            await ValidateGroupId(groupId);

            var groupMember = await _repository.GroupMember.FindWithCondition(x => x.GroupId == groupId
                                                    && x.UserId == userId
                                                    && x.IsDeleted == false);

            if(groupMember != null)
                throw new BadRequestException("You have already joined this group");

            if(isVipAccount == false)
            {
                bool isValidToJoinGroup = await _repository.Account.IsValidToJoinGroup(userId, groupId);

                if(isValidToJoinGroup == false)
                    throw new BadRequestException("You do not meet the requirements to join the group");
            }
        }

        public async Task ValidateOutGroup(Guid userId, Guid groupId)
        {
            await ValidateAccountId(userId);
            await ValidateGroupId(groupId);

            var groupMember = await _repository.GroupMember.FindWithCondition(x => x.GroupId == groupId
                                                    && x.UserId == userId
                                                    && x.IsDeleted == false);

            if(groupMember == null)
                throw new BadRequestException("You have not joined this group");
        }
    }
}