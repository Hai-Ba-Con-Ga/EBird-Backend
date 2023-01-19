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
        public static bool ValidateBirdTypeEntity(BirdTypeEntity birdType)
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
    }
}
