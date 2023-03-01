using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IGroupMemberRepository : IGenericRepository<GroupMemberEntity>
    {
        Task<ICollection<GroupMemberEntity>> GetGroupsHaveJoined(Guid userId);
        Task OutGroup(Guid userId, Guid groupId);
    }
}