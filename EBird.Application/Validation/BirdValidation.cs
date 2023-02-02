using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Application.Model.PagingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BirdValidation
    {
        public static async Task ValidateBird(BirdDTO birdDTO, IWapperRepository _repository)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO, _repository);
            //validate owner id
            await ValidateOwner(birdDTO, _repository);
        }

        public static async Task ValidateBirdType(BirdDTO birdDTO, IWapperRepository _repository)
        {
            //valid bird type id
            var birdType = await _repository.BirdType.GetBirdTypeActiveAsync(birdDTO.BirdTypeId);
            
            if(birdType == null)
            {
                throw new BadRequestException("Bird type is not exist");
            }
        }

        public static async Task ValidateOwner(BirdDTO birdDTO, IWapperRepository _repository)
        {
            //valid bird type id
            var birdType = await _repository.Account.GetByIdAsync(birdDTO.OwnerId);
            
            if(birdType == null)
            {
                throw new BadRequestException("Onwer account is not exist");
            }
        }

        public static void ValidateBirdParameter(BirdParameters parameters)
        {
            if(parameters == null)
            {
                throw new BadRequestException("Paging parameters is invalid");
            }
            if(parameters.PageNumber <= 0)
            {
                throw new BadRequestException("Page number is invalid");
            }
            if(parameters.PageSize <= 0)
            {
                throw new BadRequestException("Page size is invalid");
            }
        }
    }
}
