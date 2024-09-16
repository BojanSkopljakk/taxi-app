using AuthService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.Data;
using AuthService.repositories;


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
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            return new UserDTO
            {
                //UserId = user.UserId,
                Username = user.Username,
                //Email = user.Email,
                //Role = user.Role,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }

        public async Task<UserDTO> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.LoginAsync(request.Email, request.Password);
            if (user == null)
            {
                return null;
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return new UserDTO
            {
                Username = user.Username,
                Token = token,
                ProfilePictureUrl = user.ProfilePictureUrl
            };
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterRequest request)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hasher.HashPassword(null, request.Password),
                Role = "User",
                CreatedDate = DateTime.UtcNow,
                ProfilePictureUrl = request.ProfilePictureUrl
            };

            // Save user to database
            var registeredUser = await _userRepository.RegisterUserAsync(user);

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(registeredUser);

            // Return user data with token
            return new UserDTO
            {
                Username = registeredUser.Username,
                Token = token,
                ProfilePictureUrl = registeredUser.ProfilePictureUrl
                // Add other properties as needed
            };
        }

        public async Task<bool> UpdateProfilePictureAsync(Guid userId, string profilePictureUrl)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.ProfilePictureUrl = profilePictureUrl;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null || users.Count == 0)
            {
                return new List<UserDTO>();
            }

            return users.Select(user => new UserDTO
            {
                Username = user.Username,
                Token = _jwtTokenGenerator.GenerateToken(user),
                ProfilePictureUrl = user.ProfilePictureUrl
            }).ToList();
        }

            public async Task<bool> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.Username = request.Username ?? user.Username;
            user.Email = request.Email ?? user.Email;

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
            }

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return updatedUser != null;
        }

        public async Task<string> UploadProfilePictureAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot", "profile-pictures", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/profile-pictures/" + fileName;
        }


    }
}
