using DriverVerificationService.Models;
using DriverVerificationService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DriverVerificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverVerificationController : ControllerBase
    {
        private readonly IDriverRepository _driverRepository;

        public DriverVerificationController(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        // Endpoint for drivers to submit verification requests
        [HttpPost("request")]
        public async Task<IActionResult> SubmitVerificationRequest([FromForm] DriverVerificationRequestDTO request)
        {
            // Validate and process uploaded documents
            var documentPaths = new List<string>();
            foreach (var file in request.Documents)
            {
                // Save files to storage and add paths to documentPaths
            }

            var driver = new Driver
            {
                DriverId = request.DriverId,
                VerificationStatus = VerificationStatus.Pending,
                RegisteredAt = DateTime.UtcNow,
                //Documents = documentPaths
            };

            await _driverRepository.CreateOrUpdateDriverAsync(driver);
            return Ok();
        }

        // Endpoint for drivers to check their verification status
        [HttpGet("status/{driverId}")]
        public async Task<IActionResult> GetVerificationStatus(string driverId)
        {
            var driver = await _driverRepository.GetDriverAsync(driverId);
            if (driver == null) return NotFound();

            var response = new DriverVerificationResponseDTO
            {
                DriverId = driver.DriverId,
                VerificationStatus = driver.VerificationStatus,
                VerifiedAt = driver.VerifiedAt,
                VerificationNotes = driver.VerificationNotes
            };
            return Ok(response);
        }

        // Endpoint for admins to get pending verification requests
        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingVerifications()
        {
            var pendingDrivers = await _driverRepository.GetPendingVerificationsAsync();
            return Ok(pendingDrivers);
        }

        // Endpoint for admins to approve or reject verification requests
        [HttpPost("decision")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeVerificationDecision([FromBody] AdminVerificationDecisionDTO decision)
        {
            var driver = await _driverRepository.GetDriverAsync(decision.DriverId);
            if (driver == null) return NotFound();

            driver.VerificationStatus = decision.VerificationStatus;
            driver.VerifiedAt = decision.VerificationStatus == VerificationStatus.Approved ? DateTime.UtcNow : (DateTime?)null;
            driver.VerificationNotes = decision.VerificationNotes;

            await _driverRepository.CreateOrUpdateDriverAsync(driver);

            // Notify driver about the decision (implement notification logic)
            // ...

            return Ok();
        }
    }

}
