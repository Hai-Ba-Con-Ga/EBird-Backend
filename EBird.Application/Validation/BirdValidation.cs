using EBird.Application.Exceptions;
using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IValidation;
using EBird.Application.Model.Bird;
using EBird.Application.Model.PagingModel;
using EBird.Application.Model.Resource;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Validation
{
    public class BirdValidation : BaseValidation, IBirdValidation
    {
        public BirdValidation(IWapperRepository repository) : base(repository)
        {
        }

        public async Task ValidateCreateBird(BirdCreateDTO birdDTO)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO);
            //validate owner id
            await ValidateAccountId(birdDTO.OwnerId ?? Guid.Empty);
        }

        public async Task ValidateUpdateBird(BirdRequestDTO birdDTO, Guid birdId, Guid userId)
        {
            //valid bird type id
            await ValidateBirdType(birdDTO);

            //validate owner id
            await ValidateAccountId(userId);

            var bird = await _repository.Bird.GetByIdActiveAsync(birdId);

            if(bird == null)
            {
                throw new BadRequestException("Bird is not exist");
            }

            if(bird.OwnerId != userId)
            {
                throw new BadRequestException("You are not owner of this bird");
            }

        }

        public async Task ValidateBirdType(BirdRequestDTO birdDTO)
        {
            //valid bird type id
            var birdType = await _repository.BirdType.GetBirdTypeActiveAsync(birdDTO.BirdTypeId);

            if(birdType == null)
            {
                throw new BadRequestException("Bird type is not exist");
            }
        }

        public async Task ValidateBirdDelete(Guid userId, Guid birdId)
        {
            await ValidateAccountId(userId);

            var bird = await _repository.Bird.GetByIdActiveAsync(birdId);

            if(bird.OwnerId == userId)
            {
                throw new BadRequestException("You are not owner of this bird");
            }
        }
    }
}
