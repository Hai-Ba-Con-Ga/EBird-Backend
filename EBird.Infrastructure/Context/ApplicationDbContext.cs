using EBird.Application.Interfaces;
using EBird.Domain.Entities;
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
        public DbSet<AccountEntity> accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> refreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> verifcationStores { get; set; } = null!;

        #endregion
    }
}
