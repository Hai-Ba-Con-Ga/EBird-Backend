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
            //Config for one to many relationship between AccountEntity and RoomEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Rooms)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedById);
            //Config for one to many relationship between AccountEntity and PostEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Posts)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedById);
            //Config for one to many relationship between AccountEntity and ReportEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.HandledReports)
                .WithOne(r => r.HandledBy)
                .HasForeignKey(r => r.HandledById);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.CreatedReports)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedById);
        }

        #region DbSet
        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> VerifcationStores { get; set; } = null!;

        public DbSet<BirdEntity> Birds { get; set; }
        public DbSet<BirdTypeEntity> BirdTypes { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }

        #endregion
    }
}
