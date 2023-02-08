using System.Data;
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
            // Config for one to many relationship between AccountEntity and GroupEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(a => a.Groups)
                .WithOne(g => g.CreatedBy)
                .HasForeignKey(g => g.CreatedById);
            //Config for one to many relationship between AccountEntity and BirdEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Birds)
                .WithOne(b => b.Owner)
                .HasForeignKey(b => b.OwnerId);
            //Config for one to many relationship between AccountEntity and Rules
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Rules)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.CreateById);
            //Config for one to many relationship between AccountEntity and AccountResourceEntity
            modelBuilder.Entity<AccountResourceEntity>()
                .HasOne(m => m.Account)
                .WithMany(mt => mt.AccountResources)
                .HasForeignKey(m => m.AccountId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between AccountResourceEntity and ResourceEntity
            modelBuilder.Entity<AccountResourceEntity>()
                .HasOne(m => m.Resource)
                .WithMany(mt => mt.AccountResources)
                .HasForeignKey(m => m.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between BirdEntity and BirdResourceEntity
            modelBuilder.Entity<BirdResourceEntity>()
                .HasOne(m => m.Bird)
                .WithMany(mt => mt.BirdResources)
                .HasForeignKey(m => m.BirdId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between ResourceEntity and BirdResourceEntity
            modelBuilder.Entity<BirdResourceEntity>()
                .HasOne(m => m.Resource)
                .WithMany(mt => mt.BirdResources)
                .HasForeignKey(m => m.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship (create by) between AccountEntity and ResourceEntity
            modelBuilder.Entity<ResourceEntity>()
                .HasOne(m => m.Account)
                .WithMany(mt => mt.Resources)
                .HasForeignKey(m => m.CreateById)
                .OnDelete(DeleteBehavior.NoAction);
        }

        #region DbSet
        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> VerifcationStores { get; set; } = null!;

        public DbSet<BirdEntity> Birds { get; set; }
        public DbSet<BirdTypeEntity> BirdTypes { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RuleEntity> Rules { get; set; }

        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<AccountResourceEntity> AccountResources { get; set; }
        public DbSet<BirdResourceEntity> BirdResources { get; set; }
        #endregion
    }
}
