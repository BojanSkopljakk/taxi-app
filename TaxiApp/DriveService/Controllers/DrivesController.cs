using DriveService.Models;
using DriveService.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DriveService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrivesController : ControllerBase
    {
        private readonly IRideRepository _rideRepository;

        public DrivesController(IRideRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRide([FromBody] RideRequestDto rideRequestDTO)
        {
            var newRide = new Ride
            {
                RideId = Guid.NewGuid(),
                StartAddress = rideRequestDTO.StartAddress,
                EndAddress = rideRequestDTO.EndAddress,
                UserId = rideRequestDTO.UserId,
                Status = RideStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createdRide = await _rideRepository.CreateRideAsync(newRide);
            var response = new RideResponseDto
            {
                RideId = createdRide.RideId,
                StartAddress = createdRide.StartAddress,
                EndAddress = createdRide.EndAddress,
                UserId = createdRide.UserId,
                Status = createdRide.Status,
                CreatedAt = createdRide.CreatedAt
            };

            return Ok(response);
        }

        [HttpGet("status/{rideId}")]
        public async Task<IActionResult> GetRideStatus(Guid rideId)
        {
            var ride = await _rideRepository.GetRideAsync(rideId);
            if (ride == null) return NotFound();

            var response = new RideResponseDto
            {
                RideId = ride.RideId,
                StartAddress = ride.StartAddress,
                EndAddress = ride.EndAddress,
                Status = ride.Status,
                CreatedAt = ride.CreatedAt,
                AcceptedAt = ride.AcceptedAt,
                CompletedAt = ride.CompletedAt
            };

            return Ok(response);
        }

        private async Task<VerificationStatus> GetDriverVerificationStatusAsync(string driverId)
        {
            var driverVerificationServiceUrl = "http://localhost:5000"; // Update this URL

            using (var client = new HttpClient())
            {
                var requestUrl = $"{driverVerificationServiceUrl}/api/driververification/status/{driverId}";

                try
                {
                    var response = await client.GetAsync(requestUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response into the DTO
                        var result = await response.Content.ReadFromJsonAsync<DriverVerificationResponseDTO>();
                        return result.VerificationStatus;
                    }
                    else
                    {
                        // Handle non-success status codes as needed
                        return VerificationStatus.Pending;
                    }
                }
                catch (Exception ex)
                {
                    // Log exception
                    // For now, treat as not verified
                    return VerificationStatus.Pending;
                }
            }
        }


        [HttpPost("accept/{rideId}")]
        public async Task<IActionResult> AcceptRide(Guid rideId, [FromBody] AcceptRideDto acceptRideDTO)
        {
            // Check if driver is verified
            var driverVerificationStatus = await GetDriverVerificationStatusAsync(acceptRideDTO.DriverId);
            if (driverVerificationStatus != VerificationStatus.Approved)
            {
                return Forbid("Driver is not verified.");
            }

            var ride = await _rideRepository.GetRideAsync(rideId);
            if (ride == null) return NotFound();

            ride.DriverId = acceptRideDTO.DriverId;
            ride.Status = RideStatus.Accepted;
            ride.AcceptedAt = DateTime.UtcNow;

            await _rideRepository.UpdateRideAsync(ride);

            var response = new RideResponseDto
            {
                RideId = ride.RideId,
                StartAddress = ride.StartAddress,
                EndAddress = ride.EndAddress,
                Status = ride.Status,
                CreatedAt = ride.CreatedAt,
                AcceptedAt = ride.AcceptedAt
            };

            return Ok(response);
        }
    }
}
