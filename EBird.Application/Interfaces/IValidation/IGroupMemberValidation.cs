using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IGroupMemberValidation : IBaseValidation
    {
        Task ValidateJoinGroup(Guid userId, Guid groupId);
        Task ValidateOutGroup(Guid userId, Guid groupId);
    }
}