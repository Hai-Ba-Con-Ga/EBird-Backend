using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.PagingModel;
using EBird.Domain.Enums;

namespace EBird.Application.Model.Match
{
    public class MatchParameters : QueryStringParameters
    {
        public MatchStatus? MatchStatus { get; set; } = null;
    }
}