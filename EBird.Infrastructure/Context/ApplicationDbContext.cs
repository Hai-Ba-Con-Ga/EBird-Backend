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
            modelBuilder.Entity<AccountResourceEntity>()
                .HasOne(m => m.Account)
                .WithMany(mt => mt.AccountResources)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AccountResourceEntity>()
                .HasOne(m => m.Resource)
                .WithMany(mt => mt.AccountResources)
                .HasForeignKey(m => m.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BirdResourceEntity>()
                .HasOne(m => m.Bird)
                .WithMany(mt => mt.BirdResources)
                .HasForeignKey(m => m.BirdId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BirdResourceEntity>()
                .HasOne(m => m.Resource)
                .WithMany(mt => mt.BirdResources)
                .HasForeignKey(m => m.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ResourceEntity>()
                .HasOne(m => m.Account)
                .WithMany(mt => mt.Resources)
                .HasForeignKey(m => m.CreateById)
                .OnDelete(DeleteBehavior.NoAction);

        }

        #region DbSet
        public DbSet<AccountEntity> accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> refreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> verifcationStores { get; set; } = null!;


        public DbSet<BirdEntity> Birds { get; set; }
        public DbSet<BirdTypeEntity> BirdTypes { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<AccountResourceEntity> AccountResources { get; set; }
        public DbSet<BirdResourceEntity> BirdResources { get; set; }

        #endregion
    }
}
