using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;

namespace EBird.Application.Validation
{
    internal class RoomValidation : BaseValidation, IRoomValidation
    {
        public RoomValidation(IWapperRepository repository) : base(repository)
        {
        }
    }
}
