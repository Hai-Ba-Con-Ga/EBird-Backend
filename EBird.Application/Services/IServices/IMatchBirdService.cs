using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Interfaces.IMapper;
using EBird.Application.Model.Match;
using EBird.Domain.Entities;

namespace EBird.Application.Services.IServices
{
    public interface IMatchBirdService
    {
        public Task UpdateBirdInMatch(MatchBirdUpdateDTO matchBirdUpdateDTO);

        public Task UpdateMatchResult (UpdateMatchResultDTO matchBirdUpdateDTO);
    }
}