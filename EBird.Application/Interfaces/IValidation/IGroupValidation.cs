using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Group;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IGroupValidation : IBaseValidation
    {
        public Task ValidateGroup(GroupResponseDTO groupDTO);
        public void ValidateGroup(GroupRequestDTO groupDTO);
        
    }
}