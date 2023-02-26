using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Domain.Entities;

namespace EBird.Application.Interfaces.IRepository
{
    public interface IGroupMemberRepository : IGenericRepository<GroupMemberEntity>
    {
        Task OutGroup(Guid userId, Guid groupId);
    }
}