using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Match
{
    public class UpdateChallengerToReadyDTO
    {
        public Guid MatchId { get; set; }
        public Guid BirdId { get; set; }
        public Guid ChallengerId { get; set; }
    }
}