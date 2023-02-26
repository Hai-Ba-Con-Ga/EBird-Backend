using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.GroupMember;

namespace EBird.Application.Services.IServices
{
    public interface IGroupMemberService
    {
        Task<ICollection<GroupMemberResponseDTO>> GetGroupsHaveJoined(Guid userId);
        Task<Guid> JoinGroup(Guid userId, Guid groupId);
        Task OutGroup(Guid userId, Guid groupId);
    }
}