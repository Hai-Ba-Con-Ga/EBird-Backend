using EBird.Infrastructure.Context;
using EBird.Infrastructure.Repositories.IRepository;
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

    }
}
