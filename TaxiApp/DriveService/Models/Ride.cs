using System.Fabric.Query;

namespace DriveService.Models
{
    public class Ride
    {
        public Guid RideId { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public string UserId { get; set; }
        public string DriverId { get; set; }
        public RideStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public enum RideStatus
    {
        Pending,
        Accepted,
        Completed,
        Canceled
    }
}
