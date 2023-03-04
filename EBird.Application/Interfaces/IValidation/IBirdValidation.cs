using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.Bird;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IBirdValidation : IBaseValidation
    {
        public Task ValidateCreateBird(BirdCreateDTO birdDTO);
        public Task ValidateUpdateBird(BirdRequestDTO birdDTO, Guid birdId , Guid userId);
        public Task ValidateBirdType(BirdRequestDTO birdDTO);
        public Task ValidateBirdDelete(Guid userId, Guid birdId);
    }
}