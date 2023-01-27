using EBird.Application.Interfaces;
using EBird.Application.Interfaces.IRepository;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class WapperRepository : IWapperRepository
    {
        private readonly ApplicationDbContext _context;
        
        private IBirdTypeRepository _birdTypeRepository;

        private IBirdRepository _birdRepository;

        public WapperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBirdTypeRepository BirdType
        {
            get
            {
                if(_birdTypeRepository == null)
                {
                    _birdTypeRepository = new BirdTypeRepository(_context);
                }
                return _birdTypeRepository;
            }
        }

        public IBirdRepository Bird
        {
            get
            {
                if(_birdRepository == null)
                {
                    _birdRepository = new BirdRepository(_context);
                }
                return _birdRepository;
            }
        }

    }
}
