using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BirdTypeValidation
    {
        public static bool ValidateBirdType(BirdTypeEntity birdType)
        {
            if(birdType == null)
            {
                return false;
            }
            if(ValidateBirdTypeCode(birdType.TypeCode) 
                && ValidateBirdTypeName(birdType.TypeName)
               )
            {
                return true;
            }
            return true;
        }

        private static bool ValidateBirdTypeName(string typeName)
        {
            
        }

        public static bool ValidateBirdTypeCode(string birdTypeCode)
        {
            if(string.IsNullOrEmpty(birdTypeCode))
                return false;
            if(birdTypeCode.Trim().Length > 50)
                return false;
            return true;
        }

        public static bool ValidateBirdTypeID(Guid birdTypeID)
        {
            if(birdTypeID == null)
                return false;
            return true;
        }
        

        
    }
}
