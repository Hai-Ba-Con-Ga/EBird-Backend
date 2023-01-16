using EBird.Application.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace EBird.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        #region DbSet
        #endregion
    }
}
