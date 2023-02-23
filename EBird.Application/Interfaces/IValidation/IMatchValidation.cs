using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Match;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IMatchValidation : IBaseValidation
    {
        Task ValidateCreateMatch(MatchCreateDTO matchCreateDTO);
    }
}