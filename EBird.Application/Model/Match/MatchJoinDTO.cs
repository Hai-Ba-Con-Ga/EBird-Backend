using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Match
{
    public class MatchJoinDTO
    {
        public Guid ChallengerId { get; set; }
        public Guid BirdChallengerId { get; set; }
    }
}