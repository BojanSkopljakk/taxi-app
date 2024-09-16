namespace RatingService.Models
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public Guid RideId { get; set; }
        public string DriverId { get; set; }
        public string UserId { get; set; }
        public int Stars { get; set; } // Rating from 1 to 5
        public string Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
