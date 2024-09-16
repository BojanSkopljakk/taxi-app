﻿namespace DriveService.Models
{
    public class DriverVerificationResponseDTO
    {
        public string DriverId { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string VerificationNotes { get; set; }
    }

    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
    }


}
