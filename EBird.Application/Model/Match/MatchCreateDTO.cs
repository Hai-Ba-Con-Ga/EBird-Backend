using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Match
{
    public class MatchCreateDTO
    {
        public Guid RequestId { get; set; }
        public Guid ChallengerId { get; set; }
        public Guid ChallangerBirdId { get; set; }
    }
}