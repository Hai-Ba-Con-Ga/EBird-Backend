using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Model.Match;
using EBird.Application.Services.IServices;

namespace EBird.Application.Services
{
    public class MatchBirdService : IMatchBirdService
    {
        private IWapperRepository _repository;
        public MatchBirdService(IWapperRepository repository)
        {
            _repository = repository;
        }

        public async Task UpdateBirdInMatch(MatchBirdUpdateDTO matchBirdUpdateDTO)
        {
            var matchBird = await _repository.MatchBird.GetByIdActiveAsync(matchBirdUpdateDTO.MatchBirdId);

            if (matchBird == null)
            {
                throw new BadRequestException("Match Bird not found");
            }
            if (matchBird.MatchId != matchBirdUpdateDTO.MatchId || matchBird.BirdId != matchBirdUpdateDTO.OldBirdId)
            {
                throw new BadRequestException("Information update does not match");
            }

            matchBird.BirdId = matchBirdUpdateDTO.NewBirdId;

            await _repository.MatchBird.UpdateAsync(matchBird);
        }
    }
}