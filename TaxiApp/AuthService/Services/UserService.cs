using AuthService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.Data;


namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public Task<bool> DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> LoginAsync(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> RegisterUserAsync(Models.RegisterRequest request)
        {
            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = "User",
                CreatedDate = DateTime.UtcNow,
                ProfilePictureUrl = request.ProfilePictureUrl

            };

            // Save user to database
            await _userRepository.AddUserAsync(user);

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            // Return user data with token
            return new UserDTO { Username = user.Username, Token = token };
        }

        public Task<bool> UpdateProfilePictureAsync(Guid userId, string profilePictureUrl)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadProfilePictureAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
