using AuthenticationService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthenticationService.Models;
using System.Security.Claims;


namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(IUserRepository userRepository, IWebHostEnvironment environment)
        {
            _userRepository = userRepository;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var profileDto = new ProfileDTO
            {
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                ProfilePictureUrl = user.ProfilePictureUrl,
                UserType = user.UserType,
                VerificationStatus = user.VerificationStatus
            };

            return Ok(profileDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto model)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            user.FullName = model.FullName ?? user.FullName;
            user.DateOfBirth = model.DateOfBirth ?? user.DateOfBirth;
            user.Address = model.Address ?? user.Address;

            // Handle Profile Picture Upload
            if (model.ProfilePicture != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);
                var filePath = Path.Combine(uploads, model.ProfilePicture.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }
                user.ProfilePictureUrl = $"/uploads/{model.ProfilePicture.FileName}";
            }

            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return Ok("Profile updated successfully.");
            }

            return BadRequest(result.Errors);
        }
    }
}
