using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model;
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
            var birdType =  await _repository.BirdType.GetBirdTypeActiveAsync(birdDTO.BirdTypeId);
            if(birdType == null)
            {
                throw new BadRequestException("Bird type is not exist");
            }
        }
    }
}
