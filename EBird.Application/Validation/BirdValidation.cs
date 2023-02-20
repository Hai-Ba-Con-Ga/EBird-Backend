using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BirdValidation : BaseValidation, IBirdValidation
    {
        public BirdValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateCreateBird(BirdCreateDTO birdDTO)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO);
            //validate owner id
            await ValidateAccountId(birdDTO.OwnerId ?? Guid.Empty);
        }

        public async Task ValidateUpdateBird(BirdRequestDTO birdDTO)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO);
        }

        public async Task ValidateBirdType(BirdRequestDTO birdDTO)
        {
            //valid bird type id
            var birdType = await _repository.BirdType.GetBirdTypeActiveAsync(birdDTO.BirdTypeId);
            
            if(birdType == null)
            {
                throw new BadRequestException("Bird type is not exist");
            }
        }
    }
}
