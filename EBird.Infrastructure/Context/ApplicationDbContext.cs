using System.Data;
using EBird.Application.Interfaces;
using EBird.Domain.Entities;
using EBird.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
            //Config for NotificationTypeEntity
            modelBuilder.Entity<NotificationTypeEntity>()
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
        //Config for RequestEntity
            //Config for one to many relationship (create by) between AccountEntity and RequestEntity
            modelBuilder.Entity<RequestEntity>()
                .HasOne(m => m.CreatedBy)
                .WithMany(mt => mt.Requests)
                .HasForeignKey(m => m.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between RequestEntity and BirdEntity
            modelBuilder.Entity<RequestEntity>()
                .HasOne(m => m.Bird)
                .WithMany(mt => mt.Requests)
                .HasForeignKey(m => m.BirdId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between RequestEntity and GroupEntity
            modelBuilder.Entity<RequestEntity>()
                .HasOne(m => m.Group)
                .WithMany(mt => mt.Requests)
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to one relationship between RequestEntity and PlaceEntity
            modelBuilder.Entity<RequestEntity>()
                .HasOne(m => m.Place)
                .WithMany(mt => mt.Requests)
                .HasForeignKey(m => m.PlaceId)
                .OnDelete(DeleteBehavior.NoAction);   
            //Config HasConversation MatchEntity and Enum MatchStatus
            modelBuilder.Entity<MatchEntity>()
                .Property(m => m.MatchStatus)
                .HasConversion<string>();   
            //Config for one to many relationship between MatchEntity and PlaceEntity
            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Place)
                .WithMany(mt => mt.Matches)
                .HasForeignKey(m => m.PlaceId)
                .OnDelete(DeleteBehavior.NoAction);  
            //Config for one to many relationship between MatchEntity and MatchBirdEntity
            modelBuilder.Entity<MatchEntity>()
                .HasMany(m => m.MatchBirds)
                .WithOne(mt => mt.Match)
                .HasForeignKey(mt => mt.MatchId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between MatchEntity and AccountEntity (challenger)
            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Challenger)
                .WithMany(mt => mt.MatchesWithHost)
                .HasForeignKey(m => m.ChallengerId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between MatchEntity and AccountEntity (host)
            modelBuilder.Entity<MatchEntity>()
                .HasOne(m => m.Host)
                .WithMany(mt => mt.MatchesWithChallenger)
                .HasForeignKey(m => m.HostId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between BirdEntity and MatchBirdEntity
            modelBuilder.Entity<MatchBirdEntity>()
                .HasOne(m => m.Bird)
                .WithMany(mt => mt.MatchBirds)
                .HasForeignKey(m => m.BirdId)
                .OnDelete(DeleteBehavior.NoAction);
            //Config HasConversation MatchBirdEntity and Enum MatchBirdResult
            modelBuilder.Entity<MatchBirdEntity>()
                .Property(m => m.Result)
                .HasConversion<string>();
            //Config for one to many relationship between RequestEntity and RoomEntity
            modelBuilder.Entity<RequestEntity>()
                .HasOne(m => m.Room)
                .WithMany(mt => mt.Requests)
                .HasForeignKey(m => m.RoomId)
                .OnDelete(DeleteBehavior.NoAction);        
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Rooms)
                .WithOne(b => b.CreateBy)
                .HasForeignKey(b => b.CreateById);
            //Config for one to many relationship between AccountEntity and NotificationEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.Notifications)
                .WithOne(b => b.Account)
                .HasForeignKey(b => b.AccountId);

            //Config for one to many relationship between NotificationTypeEntity with NotificationEntity
            modelBuilder.Entity<NotificationTypeEntity>()
                .HasMany(bt => bt.Notifications)
                .WithOne(b => b.NotificationType)
                .HasForeignKey(b => b.NotificatoinTypeId);
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.ReportCreates)
                .WithOne(b => b.CreateBy)
                .HasForeignKey(b => b.CreateById)
                .OnDelete(DeleteBehavior.NoAction);;
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.ReportHandles)
                .WithOne(b => b.HandleBy)
                .HasForeignKey(b => b.HandleById)
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
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<NotificationTypeEntity> NotificationTypes { get; set; }

        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<AccountResourceEntity> AccountResources { get; set; }
        public DbSet<BirdResourceEntity> BirdResources { get; set; }

        public DbSet<ChatRoomEntity> ChatRooms { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<ParticipantEntity> Participants { get; set; }
        public DbSet<PlaceEntity> Places { get; set; }
        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<MatchEntity> Matches { get; set; }
        public DbSet<MatchBirdEntity> MatchBirds { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }
        #endregion
    }
}
