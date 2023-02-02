﻿using EBird.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Application.Services.IServices
{
    public interface IBirdService
    { 
        public Task<BirdDTO> GetBird(Guid birdID);
        public Task<List<BirdDTO>> GetBirds();
        public Task<BirdDTO> AddBird(BirdDTO birdDTO);
        public Task<BirdDTO> UpdateBird(Guid id, BirdDTO birdDTO);
        public Task<BirdDTO> DeleteBird(Guid birdID);
        public Task<List<BirdDTO>> GetAllBirdByAccount(Guid accountId);

    }
}
