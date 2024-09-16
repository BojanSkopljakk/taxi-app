using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RatingService.Models;
using RatingService.Repositories;
using System.Threading.Tasks;

namespace RatingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingsController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        /// <summary>
        /// Allows a user to submit a rating for a driver after a ride.
        /// </summary>
        [HttpPost("submit")]
        [Authorize] // Ensure that only authenticated users can submit ratings
        public async Task<IActionResult> SubmitRating([FromBody] SubmitRatingDTO submitRatingDTO)
        {
            // Validate the rating input
            if (submitRatingDTO.Stars < 1 || submitRatingDTO.Stars > 5)
            {
                return BadRequest("Rating must be between 1 and 5 stars.");
            }

            if (submitRatingDTO.RideId == Guid.Empty || string.IsNullOrEmpty(submitRatingDTO.DriverId))
            {
                return BadRequest("RideId and DriverId are required.");
            }

            // Get the user ID from the authenticated user
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if the user has already submitted a rating for this ride
            var existingRating = await _ratingRepository.GetRatingByRideAndUserAsync(submitRatingDTO.RideId, userId);
            if (existingRating != null)
            {
                return BadRequest("You have already submitted a rating for this ride.");
            }

            // Optionally, verify that the ride exists and is completed (requires integration with DriveService)
            // For now, we assume the ride is valid and completed

            var rating = new Rating
            {
                RatingId = Guid.NewGuid(),
                RideId = submitRatingDTO.RideId,
                DriverId = submitRatingDTO.DriverId,
                UserId = userId,
                Stars = submitRatingDTO.Stars,
                Feedback = submitRatingDTO.Feedback,
                CreatedAt = DateTime.UtcNow
            };

            await _ratingRepository.SubmitRatingAsync(rating);

            return Ok("Rating submitted successfully.");
        }

        /// <summary>
        /// Retrieves a driver's average rating and total number of ratings.
        /// Accessible only by admins.
        /// </summary>
        [HttpGet("driver/{driverId}/summary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDriverRatingSummary(string driverId)
        {
            if (string.IsNullOrEmpty(driverId))
            {
                return BadRequest("DriverId is required.");
            }

            var summary = await _ratingRepository.GetDriverRatingSummaryAsync(driverId);

            if (summary == null)
            {
                return NotFound("Driver not found or no ratings available.");
            }

            var response = new DriverRatingDTO
            {
                DriverId = summary.DriverId,
                AverageRating = summary.AverageRating,
                TotalRatings = summary.TotalRatings
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieves all ratings for a specific driver.
        /// Accessible only by admins.
        /// </summary>
        [HttpGet("driver/{driverId}/ratings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDriverRatings(string driverId)
        {
            if (string.IsNullOrEmpty(driverId))
            {
                return BadRequest("DriverId is required.");
            }

            var ratings = await _ratingRepository.GetDriverRatingsAsync(driverId);

            return Ok(ratings);
        }

        /// <summary>
        /// Retrieves all ratings submitted by the authenticated user.
        /// </summary>
        [HttpGet("myratings")]
        [Authorize]
        public async Task<IActionResult> GetUserRatings()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var ratings = await _ratingRepository.GetUserRatingsAsync(userId);

            return Ok(ratings);
        }
    }
}
