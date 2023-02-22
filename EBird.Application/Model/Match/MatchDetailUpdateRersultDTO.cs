using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBird.Application.Model.Match
{
    public class MatchDetailUpdateResultDTO
    {
        public Guid BirdId { get; set; }
        public string Result { get; set; }
    }
}