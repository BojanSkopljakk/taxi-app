namespace RatingService.Models
{
    public class DriverRatingSummary
    {
        public string DriverId { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }

}
