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
        public Task<GroupResponseDTO> GetGroup(Guid groupId);
        public Task<List<GroupResponseDTO>> GetGroups();
        public Task AddGroup(GroupCreateDTO groupDTO);
        public Task UpdateGroup(Guid groupId, GroupRequestDTO groupDTO);
        public Task DeleteGroup(Guid groupId);
    }
}
