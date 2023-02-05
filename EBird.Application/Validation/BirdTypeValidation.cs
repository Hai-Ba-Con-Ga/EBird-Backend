using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.BirdType;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BirdTypeValidation
    {
        public static async Task ValidateBirdTypeDTO(BirdTypeRequestDTO birdType, IWapperRepository _repository)
        {
            if(birdType == null)
            {
                throw new BadRequestException("Data is null");
            }

            var result = await ValidateBirdTypeCode(birdType.TypeCode, _repository);

            if(result == false)
            {
                throw new BadRequestException("Bird type code already exists");
            }
            
        }

        public static async Task<bool> ValidateBirdTypeCode(string birdTypeCode, IWapperRepository _repository)
        {
            bool isExist = await _repository.BirdType.IsExistBirdTypeCode(birdTypeCode);
            if(isExist == false)
            {
                return true;
            }
            return false;
        }
    }
}
