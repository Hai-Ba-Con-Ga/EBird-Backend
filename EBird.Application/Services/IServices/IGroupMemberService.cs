using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IGroupMemberService
    {
        Task<Guid> JoinGroup(Guid userId, Guid groupId);
        Task OutGroup(Guid userId, Guid groupId);
    }
}