using EBird.Application.Interfaces;
using EBird.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBird.Infrastructure.Repositories
{
    /// <summary>
    /// This class help to manage repositories
    /// </summary>
    internal class WapperRepository : IWapperRepository
    {
        private ApplicationDbContext _context;

        public WapperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
           Example for using this class:
           - Initialize a repository class for using 
               private IProductRepository _product;

               public IProductRepository 
               {
                   get
                   {
                       if(_product == null)
                       {
                           _product = new ProductRepository(_context);
                       }
                       return _product;
                   }
               }
           - Then, u can use whatever repository u want throught this class by: 
                    WapperRepository repository = new WapperRepository(dbContext);
                   repository.[nameRepository].[method]
        */


    }
}
