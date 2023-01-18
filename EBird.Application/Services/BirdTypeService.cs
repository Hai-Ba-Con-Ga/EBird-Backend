using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBird.Infrastructure.Repositories;
using System.Security;
using EBird.Infrastructure.Repositories.IRepository;

namespace EBird.Application.Services
{
    public class BirdTypeService : IBirdTypeService
    {
        
       
        public Task<Response<BirdTypeEntity>> DeleteBirdType(int birdTypeID)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BirdTypeEntity>> DeleteBirdType(string birdTypeCode)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<BirdTypeEntity>>> GetAllBirdType()
        {
            throw new NotImplementedException();
        }

        public Task<Response<BirdTypeEntity>> GetBirdType(string birdTypeCode)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BirdTypeEntity>> GetBirdType(int birdTypeID)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BirdTypeEntity>> InsertBirdType(BirdTypeEntity birdType)
        {
            throw new NotImplementedException();
        }

        public Task<Response<BirdTypeEntity>> UpdateBirdType(BirdTypeEntity birdType)
        {
            throw new NotImplementedException();
        }
    }
}
