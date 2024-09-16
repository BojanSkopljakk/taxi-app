using RatingService.Data;
using RatingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RatingService.Repositories
{


    public class RatingRepository : IRatingRepository
    {
        private readonly RatingDbContext _context;

        public RatingRepository(RatingDbContext context)
        {
            _context = context;
        }

        public async Task SubmitRatingAsync(Rating rating)
        {
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<DriverRatingSummary> GetDriverRatingSummaryAsync(string driverId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.DriverId == driverId)
                .ToListAsync();

            var summary = new DriverRatingSummary
            {
                DriverId = driverId,
                AverageRating = ratings.Any() ? ratings.Average(r => r.Stars) : 0,
                TotalRatings = ratings.Count
            };

            return summary;
        }

        public async Task<IEnumerable<Rating>> GetDriverRatingsAsync(string driverId)
        {
            return await _context.Ratings
                .Where(r => r.DriverId == driverId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Rating> GetRatingByRideAndUserAsync(Guid rideId, string userId)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.RideId == rideId && r.UserId == userId);
        }

        // Add this method
        public async Task<IEnumerable<Rating>> GetUserRatingsAsync(string userId)
        {
            return await _context.Ratings
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }

}
