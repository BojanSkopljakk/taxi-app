namespace RatingService.Models
{
    public class DriverRatingDTO
    {
        public string DriverId { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }

}
