using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class GroupValidation : BaseValidation
    {
        public static async Task ValidateGroup(GroupResponseDTO groupDTO, IWapperRepository _repository)
        {
            //validate account id
            await ValidateAccountId(groupDTO.CreatedById, _repository);
            //validate maxELO > minELO
            if(groupDTO.MaxELO <= groupDTO.MinELO)
            {
                throw new BadRequestException("Max ELO must be greater than Min ELO");
            }
        }

        public static async Task ValidateGroup(GroupRequestDTO groupDTO, IWapperRepository _repository)
        {
            //validate maxELO > minELO
            if(groupDTO.MaxELO <= groupDTO.MinELO)
            {
                throw new BadRequestException("Max ELO must be greater than Min ELO");
            }
        }
    }
}
