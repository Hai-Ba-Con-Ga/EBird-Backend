using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class GroupRepository : GenericRepository<GroupEntity>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<GroupEntity> AddGroupAsync(GroupEntity group)
        {
            group.CreateDatetime = DateTime.Now;
            var result =  await this.CreateAsync(group);
            if(result == 0)
            {
                return null;
            }
            return group;
        }

        public async Task<GroupEntity> GetGroupActiveAsync(Guid groupID)
        {
            return await this.GetByIdActiveAsync(groupID);
        }

        public async Task<List<GroupEntity>> GetGroupsActiveAsync()
        {
            return await this.GetAllActiveAsync();
        }

        public async Task<GroupEntity> SoftDeleteGroupAsync(Guid groupID)
        {
            var group = await this.GetByIdActiveAsync(groupID);

            if(group == null)
            {
                return null;
            }
            
           return  await this.DeleteSoftAsync(groupID);
        }

        public async Task<int> UpdateGroupAsync(GroupEntity group)
        {
            var result = await this.UpdateAsync(group);
            return result;
        }
    }
}
