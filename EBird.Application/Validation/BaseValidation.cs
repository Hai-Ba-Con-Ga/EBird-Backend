using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public abstract class BaseValidation
    {
        public static bool ValidateID(Guid id)
        {
            if(id == null)
                return false;
            return true;
        }

        public static bool ValidateDate(DateTime date)
        {
            if(date == null)
                return false;
            return true;
        }
        
    }
}
