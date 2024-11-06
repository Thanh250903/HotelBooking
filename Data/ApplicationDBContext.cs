
using HotelApp.Models.Hotel;
using HotelApp.Models.Others;
using HotelApp.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelReview> HotelReviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<SendEmail> SendEmails { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                        .HasOne(p => p.RoomBooking) // tham chiếu đến roombooking
                        .WithMany() // không cần chỉ định một collection ở đây
                        .HasForeignKey(p => p.RoomBookingId) // foreign key in Payment
                        .OnDelete(DeleteBehavior.Restrict); // Tắt hành vi xóa cascade từ Payment đến RoomBooking
            modelBuilder.Entity<SendEmail>().HasNoKey();
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
