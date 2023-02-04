using EBird.Application.Model.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IGroupService
    {
        public Task<GroupDTO> GetGroup(Guid groupId);
        public Task<List<GroupDTO>> GetGroups();
        public Task<GroupDTO> AddGroup(GroupDTO groupDTO);
        public Task<GroupDTO> UpdateGroup(Guid groupId, GroupUpdateDTO groupUpdateDTO);
        public Task<GroupDTO> DeleteGroup(Guid groupId);
    }
}
