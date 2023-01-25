using EBird.Application.Interfaces;
using EBird.Application.Model;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BirdTypeValidation : BaseValidation
    {
        public static bool ValidateBirdTypeEntity( IWapperRepository _repository, BirdTypeEntity birdType)
        {
            if(birdType == null)
            {
                return false;
            }
            if(ValidateID(birdType.Id) == false)
            {
                return false;
            }
            if(ValidateString(birdType.TypeCode, 50) == false)
            {
                return false;
            }
            if(ValidateBirdTypeCode(_repository, birdType.TypeCode) == false)
            {
                return false;
            }
            if(ValidateString(birdType.TypeName, 100) == false)
            {
                return false;
            }
            if(ValidateDatetime(birdType.CreatedDatetime) == false)
            {
                return false;
            }
            if(birdType.IsDeleted == true)
            {
                return false;
            }
            return true;
            
        }

        public static bool ValidateBirdTypeDTO(BirdTypeDTO birdType)
        {
            if(birdType == null)
            {
                return false;
            }
            if(ValidateString(birdType.TypeCode, 50) == false)
            {
                return false;
            }
            if(ValidateString(birdType.TypeName, 100) == false)
            {
                return false;
            }
            return true;

        }

        public static bool ValidateBirdTypeCode(IWapperRepository _repository, string birdTypeCode)
        {
            bool isExist = _repository.BirdType.IsExistBirdTypeCode(birdTypeCode);
            if(isExist == false)
            {
                return true;
            }
            return false;
        }
    }
}
