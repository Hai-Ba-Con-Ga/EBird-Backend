using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IUnitOfValidation
    {
        public IBaseValidation Base { get; }
        public IGroupValidation Group { get; }
        public IRequestValidation Request { get; }
        public IRoomValidation Room { get; }
        public IBirdValidation Bird { get; }
        public IBirdTypeValidation BirdType { get; }
        public IMatchValidation Match { get; }
    }
}