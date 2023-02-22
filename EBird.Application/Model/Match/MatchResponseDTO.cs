using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Bird;
using EBird.Application.Model.Place;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Match
{
    public class MatchResponseDTO : IMapFrom<MatchEntity>
    {
        public Guid Id { get; set; }
        public string MatchDatetime { get; set; }
        public string CreateDatetime { get; set; }
        public MatchStatus MatchStatus { get; set; }
        public Guid HostId { get; set; }
        public Guid ChallengerId { get; set; }
        public Guid PlaceId { get; set; }
        public PlaceResponseDTO Place { get; set; }
        public ICollection<MatchDetailResponseDTO> MatchBirdList { get; set; }
        
    }
}