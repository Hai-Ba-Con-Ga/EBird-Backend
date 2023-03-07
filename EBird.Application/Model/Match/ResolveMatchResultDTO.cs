using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Model.Match
{
    public class ResolveMatchResultDTO
    {
        public Guid winBirdId { get; set; }
        public Guid loseBirdId { get; set; }
    }
}
