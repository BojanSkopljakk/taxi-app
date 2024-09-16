namespace DriverVerificationService.Models
{
    public class AdminVerificationDecisionDTO
    {
        public string DriverId { get; set; }
        public VerificationStatus VerificationStatus { get; set; } // Approved or Rejected
        public string VerificationNotes { get; set; }
    }

}
