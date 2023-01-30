using EBird.Application.Interfaces.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Interfaces
{
    public interface IWapperRepository
    {
        public IBirdTypeRepository BirdType { get; }
        public IBirdRepository Bird { get; }
    }
}
