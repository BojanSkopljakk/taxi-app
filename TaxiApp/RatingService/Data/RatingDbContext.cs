using Microsoft.EntityFrameworkCore;
using RatingService.Models;

namespace RatingService.Data
{
    public class RatingDbContext : DbContext
    {
        public RatingDbContext(DbContextOptions<RatingDbContext> options)
            : base(options)
        {
        }

        // DbSet for the Rating entity
        public DbSet<Rating> Ratings { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entities using Fluent API if needed

            // For example, configure the Rating entity
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(r => r.RatingId);

                entity.Property(r => r.Stars)
                    .IsRequired();

                entity.Property(r => r.Feedback)
                    .HasMaxLength(1000);

                // Indexes for faster queries
                entity.HasIndex(r => r.DriverId);
                entity.HasIndex(r => r.UserId);
                entity.HasIndex(r => r.RideId);
            });

            base.OnModelCreating(modelBuilder);
        }*/
    }
}
