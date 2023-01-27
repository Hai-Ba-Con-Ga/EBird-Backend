using EBird.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IBirdService
    { 
        public Task<Response<BirdDTO>> GetBird(Guid birdID);
        public Task<Response<List<BirdDTO>>> GetBirds();
        public Task<Response<BirdDTO>> AddBird(BirdDTO birdDTO);
        public Task<Response<BirdDTO>> UpdateBird(Guid id, BirdDTO birdDTO);
        public Task<Response<BirdDTO>> DeleteBird(Guid birdID);

    }
}
