using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Place;
using EBird.Domain.Entities;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Match
{
    public class MatchUpdateDTO : IMapTo<MatchEntity>
    {
        virtual public PlaceRequestDTO? Place { get; set; }
        public MatchStatus? MatchStatus { get; set; }
        public DateTime? MatchDatetime { get; set; }
        public Guid? ChallengerId { get; set; }
        public Guid? BirdChallengerId { get; set; }
    }
}