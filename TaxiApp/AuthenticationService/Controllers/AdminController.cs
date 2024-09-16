using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthenticationService.Models;
using AuthenticationService.Repositories;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly IDriverApplicationRepository _driverApplicationRepository;
        private readonly IUserRepository _userRepository;

        public AdminController(IDriverApplicationRepository driverApplicationRepository, IUserRepository userRepository)
        {
            _driverApplicationRepository = driverApplicationRepository;
            _userRepository = userRepository;
        }

        [HttpGet("PendingVerifications")]
        public async Task<IActionResult> GetPendingVerifications()
        {
            var pendingDrivers = await _driverApplicationRepository.GetPendingDriverVerificationsAsync();
            return Ok(pendingDrivers);
        }

        [HttpPost("VerifyDriver")]
        public async Task<IActionResult> VerifyDriver([FromBody] DriverVerificationDto model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.UserId);
            if (user == null || user.UserType != UserType.Driver)
                return NotFound("Driver not found.");

            if (user.VerificationStatus != VerificationStatus.Pending)
                return BadRequest("Driver has already been processed.");

            await _driverApplicationRepository.UpdateVerificationStatusAsync(user, model.IsApproved);

            return Ok(new { message = "Driver verification updated." });
        }
    }
}
