namespace AuthService.Models
{
    public class UpdateUserProfileRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }

        // Optional: If the user wants to update their profile picture
        public IFormFile ProfilePicture { get; set; }
    }

}
