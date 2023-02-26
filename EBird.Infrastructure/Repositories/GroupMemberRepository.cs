using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EBird.Infrastructure.Repositories
{
    public class GroupMemberRepository : GenericRepository<GroupMemberEntity>, IGroupMemberRepository
    {
        public GroupMemberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task OutGroup(Guid userId, Guid groupId)
        {
            var groupMember = await _context.GroupMembers
                        .Where(x => x.GroupId == groupId
                                && x.UserId == userId
                                && x.IsDeleted == false).FirstOrDefaultAsync();

            if (groupMember != null)
            {
                groupMember.IsDeleted = true;
                _context.GroupMembers.Update(groupMember);
                await _context.SaveChangesAsync();
            }
        }
    }
}