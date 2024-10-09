
using HotelApp.Models.Hotel;
using HotelApp.Models.Others;
using HotelApp.Models.Room;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelReview> HotelReviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ApplicationUser> User { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }
    }

}
