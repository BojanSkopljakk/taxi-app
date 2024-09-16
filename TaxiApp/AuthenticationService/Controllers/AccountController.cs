using AuthenticationService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.Models;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;


        public AccountController(IUserRepository userRepository, IWebHostEnvironment environment, IConfiguration configuration, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _environment = environment;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (userExists != null)
                return BadRequest("Username already exists.");

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                UserType = model.UserType,
                VerificationStatus = model.UserType == UserType.Driver ? VerificationStatus.Pending : VerificationStatus.NotRequired
            };

            // Handle Profile Picture Upload (as before)
            // Handle Profile Picture Upload
            /*if (model.ProfilePicture != null)
            {
                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);
                var filePath = Path.Combine(uploads, model.ProfilePicture.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }*/
                user.ProfilePictureUrl = $"/uploads/{model.ProfilePicture.FileName}";
           // }

            var result = await _userRepository.CreateUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign role
                var role = model.UserType.ToString();
                await _userRepository.AddToRoleAsync(user, role);

                // Send confirmation email if needed
                // await SendConfirmationEmail(user);

                return Ok(new { message = "Registration successful. Please wait for administrator approval if you registered as a driver." });
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return BadRequest(ModelState);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (user == null || !await _userRepository.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid username or password.");

            var roles = await _userRepository.GetRolesAsync(user);

            // Generate the JWT token using TokenRepository
            var token = _tokenRepository.GenerateToken(user, roles);

            return Ok(new { token });
        }


    }
}
