using AuthService.Models;
//using Microsoft.AspNetCore.Identity.Data;

namespace AuthService.Services
{
    public interface IUserService
    {
        // Registers a new user
        Task<UserDTO> RegisterUserAsync(RegisterRequest request);

        // Authenticates a user and returns a token
        Task<UserDTO> LoginAsync(LoginRequest request);

        // Retrieves a user profile by user ID
        Task<UserDTO> GetUserByIdAsync(Guid userId);

        // Updates user profile details (including profile picture)
        Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request);

        // Deletes a user account
        Task<bool> DeleteUserAsync(Guid userId);

        // Uploads a profile picture and returns the URL
        Task<string> UploadProfilePictureAsync(IFormFile file);

        // Updates the profile picture of a user
        Task<bool> UpdateProfilePictureAsync(Guid userId, string profilePictureUrl);
    }

}
