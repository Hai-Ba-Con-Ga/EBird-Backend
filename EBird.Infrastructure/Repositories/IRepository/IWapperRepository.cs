using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories.IRepository
{
    public interface IWapperRepository
    {
        public IBirdTypeRepository BirdType { get; }
    }
}
