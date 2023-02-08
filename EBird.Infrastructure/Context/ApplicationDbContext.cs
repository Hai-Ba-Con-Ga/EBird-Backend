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
            //Config for one to many relationship "create" between AccountEntity and Resource
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Resources)
                .WithOne(r => r.CreateBy)
                .HasForeignKey(r => r.CreateById);
            //Config for many to many relationship between BirdEntity and Resource
            modelBuilder.Entity<BirdEntity>()
                .HasMany(b => b.BirdResources)
                .WithOne(br => br.BirdEntity)
                .HasForeignKey(br => br.BirdId);
            modelBuilder.Entity<ResourceEntity>()
                .HasMany(r => r.BirdResource)
                .WithOne(br => br.ResourceEntity)
                .HasForeignKey(br => br.ResourceId);
            modelBuilder.Entity<BirdResource>()
                .HasIndex(br => new {br.BirdId, br.ResourceId});
            //config for many to many relationship between AccountEntity and Resource
            modelBuilder.Entity<AccountEntity>()
                .HasMany(a => a.Account_Resource)
                .WithOne(ar => ar.AccountEntity)
                .HasForeignKey(ar => ar.AccountId);
            modelBuilder.Entity<ResourceEntity>()
                .HasMany(r => r.BirdResource)
                .WithOne(ar => ar.ResourceEntity)
                .HasForeignKey(ar => ar.ResourceId);
            modelBuilder.Entity<AccountResource>()
                .HasIndex(ar => new {ar.AccountId, ar.ResourceId});
        }

        #region DbSet
        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> VerifcationStores { get; set; } = null!;

        public DbSet<BirdEntity> Birds { get; set; }
        public DbSet<BirdTypeEntity> BirdTypes { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<BirdResource> Bird_Resources { get; set; }
        public DbSet<AccountResource> Account_Resources { get; set; }
        #endregion
    }
}
