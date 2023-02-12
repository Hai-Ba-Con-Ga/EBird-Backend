using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
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
    public class BirdValidation : BaseValidation
    {
        public static async Task ValidateCreateBird(BirdCreateDTO birdDTO, IWapperRepository _repository)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO, _repository);
            //validate owner id
            await ValidateAccountId(birdDTO.OwnerId, _repository);
        }

        public static async Task ValidateUpdateBird(BirdRequestDTO birdDTO, IWapperRepository _repository)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO, _repository);
        }

        public static async Task ValidateBirdType(BirdRequestDTO birdDTO, IWapperRepository _repository)
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
