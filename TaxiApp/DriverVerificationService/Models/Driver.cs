namespace DriverVerificationService.Models
{
    public class Driver
    {
        public string DriverId { get; set; } // Could be the same as UserId
        public string Name { get; set; }
        public string Email { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string VerificationNotes { get; set; }
        public bool IsBlocked { get; set; }
       // public List<string> Documents { get; set; } // Paths or identifiers for uploaded documents
    }

    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
    }

}
