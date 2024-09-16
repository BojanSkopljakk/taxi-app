using RatingService.Models;

namespace RatingService.Repositories
{
    public interface IRatingRepository
    {
        Task SubmitRatingAsync(Rating rating);
        Task<DriverRatingSummary> GetDriverRatingSummaryAsync(string driverId);
        Task<IEnumerable<Rating>> GetDriverRatingsAsync(string driverId);

        Task<Rating> GetRatingByRideAndUserAsync(Guid rideId, string userId); // Add this method
        Task<IEnumerable<Rating>> GetUserRatingsAsync(string userId); // Add this method
    }

}
