using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    public class DriverVerificationDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public bool IsApproved { get; set; }
    }
}
