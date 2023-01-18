using EBird.Application.Services.IServices;
using EBird.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;

using EBird.Application.Interfaces.IRepository;
using EBird.Application.Interfaces;
using EBird.Application.Model;

namespace EBird.Application.Services
{
    public class BirdTypeService : IBirdTypeService
    {
        private IWapperRepository _wapperRepository;

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
