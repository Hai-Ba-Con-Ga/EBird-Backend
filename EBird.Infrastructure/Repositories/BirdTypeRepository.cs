using EBird.Application.Interfaces.IRepository;
using EBird.Domain.Entities;
using EBird.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    public class BirdTypeRepository : GenericRepository<BirdTypeEntity>, IBirdTypeRepository
    {
        public BirdTypeRepository(ApplicationDbContext context) : base(context)
        {
        }


        
    }

}
