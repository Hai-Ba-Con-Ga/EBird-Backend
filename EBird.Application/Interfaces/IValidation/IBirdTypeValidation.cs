using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBird.Application.Model.BirdType;

namespace EBird.Application.Interfaces.IValidation
{
    public interface IBirdTypeValidation : IBaseValidation
    {
        public Task ValidateBirdTypeDTO(BirdTypeRequestDTO birdType);
        public Task<bool> ValidateBirdTypeCode(string birdTypeCode);
    }
}