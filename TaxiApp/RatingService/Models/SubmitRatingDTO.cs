namespace RatingService.Models
{
    public class SubmitRatingDTO
    {
        public Guid RideId { get; set; }
        public string DriverId { get; set; }
        public int Stars { get; set; } // Rating from 1 to 5
        public string Feedback { get; set; }
    }

}
