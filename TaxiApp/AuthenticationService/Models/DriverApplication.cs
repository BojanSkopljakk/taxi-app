using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    public class DriverApplication
    {
        [Key]
        public int ApplicationId { get; set; }
        public string UserId { get; set; } // FK to User

        public string LicenseNumber { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleRegistrationNumber { get; set; }

        public DateTime ApplicationDate { get; set; }
        public VerificationStatus Status { get; set; }
    }
}
