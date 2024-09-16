using DriveService.Models;

namespace DriveService.Repositories
{
    public interface IRideRepository
    {
        Task<Ride> CreateRideAsync(Ride ride);
        Task<Ride> GetRideAsync(Guid rideId);
        Task UpdateRideAsync(Ride ride);
        Task<IEnumerable<Ride>> GetPendingRidesAsync();
    }

}
