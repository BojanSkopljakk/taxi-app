namespace DriveService.Models
{
    public class RideRequestDto
    {
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public string UserId { get; set; }
    }
}
