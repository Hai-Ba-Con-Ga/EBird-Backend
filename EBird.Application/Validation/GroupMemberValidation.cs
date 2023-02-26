using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;

namespace EBird.Application.Validation
{
    public class GroupMemberValidation : BaseValidation, IGroupMemberValidation
    {
        public GroupMemberValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateJoinGroup(Guid userId, Guid groupId)
        {
            await ValidateAccountId(userId);
            await ValidateGroupId(groupId);

            var groupMember = await _repository.GroupMember.FindWithCondition(x => x.GroupId == groupId
                                                    && x.UserId == userId
                                                    && x.IsDeleted == false);

            if (groupMember != null)
                throw new BadRequestException("You have already joined this group");
        }

        public async Task ValidateOutGroup(Guid userId, Guid groupId)
        {
            await ValidateAccountId(userId);
            await ValidateGroupId(groupId);

            var groupMember = await _repository.GroupMember.FindWithCondition(x => x.GroupId == groupId
                                                    && x.UserId == userId
                                                    && x.IsDeleted == false);

            if (groupMember == null)
                throw new BadRequestException("You have not joined this group");
        }
    }
}