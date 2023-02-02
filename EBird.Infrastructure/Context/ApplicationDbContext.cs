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
            //Config for BirdTypeEnitty
            modelBuilder.Entity<BirdTypeEntity>()
                .HasIndex(b => b.TypeCode)
                .IsUnique(true);

            //Config for one to many relationship between BirdTypeEntity and BirdEntity
            modelBuilder.Entity<BirdTypeEntity>()
                .HasMany(bt => bt.Birds)
                .WithOne(b => b.BirdType)
                .HasForeignKey(b => b.BirdTypeId);
        }

        #region DbSet
        public DbSet<AccountEntity> accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> refreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> verifcationStores { get; set; } = null!;

        public DbSet<BirdEntity> Birds { get; set; }
        public DbSet<BirdTypeEntity> BirdTypes { get; set; }

        #endregion
    }
}
