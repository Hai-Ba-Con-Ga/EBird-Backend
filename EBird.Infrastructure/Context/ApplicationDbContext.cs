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
        public DbSet<RoomEntity> Rooms { get; set; }
        #endregion
    }
}
