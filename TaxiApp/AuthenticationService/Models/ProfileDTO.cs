namespace AuthenticationService.Models
{
    public class ProfileDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
        public UserType UserType { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
    }
}
