using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Bird;
using EBird.Application.Model.Resource;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Match
{
    public class MatchDetailResponseDTO : IMapFrom<MatchDetailEntity>
    {
        public Guid Id { get; set; }
        public BirdResponseDTO Bird { get; set; }
        public int AfterElo { get; set; }
        public int BeforeElo { get; set; }
        public MatchDetailResult Result { get; set; }
        public DateTime UpdateDatetime { get; set; }
        public ICollection<ResourceResponse>? ResourceResponses { get; set; }

    }
}