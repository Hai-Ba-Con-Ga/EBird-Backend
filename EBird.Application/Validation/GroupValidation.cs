using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class GroupValidation : BaseValidation , IGroupValidation
    {
        public GroupValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateGroup(GroupResponseDTO groupDTO)
        {
            //validate account id
            await ValidateAccountId(groupDTO.CreatedById);
            //validate maxELO > minELO
            if(groupDTO.MaxELO <= groupDTO.MinELO)
            {
                throw new BadRequestException("Max ELO must be greater than Min ELO");
            }
        }

        public void ValidateGroup(GroupRequestDTO groupDTO)
        {
            //validate maxELO > minELO
            if(groupDTO.MaxELO <= groupDTO.MinELO)
            {
                throw new BadRequestException("Max ELO must be greater than Min ELO");
            }
        }
    }
}
