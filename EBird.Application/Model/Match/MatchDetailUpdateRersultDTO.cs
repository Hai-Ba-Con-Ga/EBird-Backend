using EBird.Application.Model.Resource;
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
        public List<ResourceCreateDTO>? ListResource { get; set; }
    }
}