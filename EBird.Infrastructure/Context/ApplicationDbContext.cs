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
                .HasForeignKey(b => b.BirdTypeId)
                .OnDelete(DeleteBehavior.NoAction);


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
                .HasForeignKey(r => r.HandledById)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.CreatedReports)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
            //Config for one to many relationship between AccountEntity and FriendshipEntity
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.InviterFriendships)
                .WithOne(r => r.Inviter)
                .HasForeignKey(r => r.InviterId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.RecieverFriendships)
                .WithOne(r => r.Reciever)
                .HasForeignKey(r => r.RecieverId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.SentMessagePrivates)
                .WithOne(r => r.Sent)
                .HasForeignKey(r => r.SentId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<AccountEntity>()
                .HasMany(acc => acc.ReceiveMessagePrivates)
                .WithOne(r => r.Receive)
                .HasForeignKey(r => r.ReceiveId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<RoomEntity>()
                .HasMany(room => room.Groups)
                .WithOne(r => r.Room)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<RoomEntity>()
                .HasMany(room => room.Requests)
                .WithOne(r => r.Room)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BirdEntity>()
                .HasMany(b => b.MatchBirds)
                .WithOne(r => r.Bird)
                .HasForeignKey(r => r.BirdId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<GroupEntity>()
                .HasMany(g => g.GroupMembers)
                .WithOne(r => r.Group)
                .HasForeignKey(r => r.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<GroupEntity>()
                .HasMany(g => g.Requests)
                .WithOne(r => r.Group)
                .HasForeignKey(r => r.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<GroupEntity>()
                .HasMany(g => g.SentMessageGroups)
                .WithOne(r => r.Sent)
                .HasForeignKey(r => r.SentId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<GroupEntity>()
                .HasMany(g => g.ReceiveMessageGroups)
                .WithOne(r => r.Receive)
                .HasForeignKey(r => r.ReceiveId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<BirdEntity>()
                .HasMany(g => g.Requests)
                .WithOne(r => r.Bird)
                .HasForeignKey(r => r.BirdId)
                .OnDelete(DeleteBehavior.NoAction);




        }

        #region DbSet
        public DbSet<AccountEntity> Accounts { get; set; } = null!;
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
        public DbSet<VerifcationStoreEntity> VerifcationStores { get; set; } = null!;

        public DbSet<AccountResourceEntity> AccountResources { get; set; }

        public DbSet<BirdEntity> Birds { get; set; }
        public DbSet<BirdResourceEntity> BirdResources { get; set; }
        public DbSet<BirdTypeEntity> BirdTypes { get; set; }

        public DbSet<FriendshipEntity> Friendships { get; set; }

        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<GroupMemberEntity> GroupMembers { get; set; }

        public DbSet<MatchBirdEntity> MatchBirds { get; set; }
        public DbSet<MatchEntity> Matchs { get; set; }
        public DbSet<MatchMessageEntity> MatchMessages { get; set; }
        public DbSet<MatchResourceEntity> MatchResources { get; set; }

        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<MessageGroupEntity> MessageGroups { get; set; }
        public DbSet<MessagePrivateEntity> MessagePrivates { get; set; }

        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<NotificationTypeEntity> NotificationTypes { get; set; }
        
        public DbSet<PlaceEntity> Places { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<RequestEntity> Requests { get; set; }
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RuleEntity> Rules { get; set; }

        #endregion
    }
}
