namespace AuthService.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // User, Driver, Admin

        public DateTime BornDate { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
