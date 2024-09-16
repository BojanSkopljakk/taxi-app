using Microsoft.AspNetCore.Identity;


namespace AuthenticationService.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } // Combination of First Name and Last Name
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
        public UserType UserType { get; set; } // Enum: Administrator, User, Driver
        public VerificationStatus VerificationStatus { get; set; } // For Drivers
    }


    public enum UserType
    {
        Admin,
        User,
        Driver
    }

    public enum VerificationStatus
    {
        NotRequired,
        Pending,
        Approved,
        Rejected
    }
}
