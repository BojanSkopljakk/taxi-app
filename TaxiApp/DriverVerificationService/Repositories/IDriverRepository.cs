using DriverVerificationService.Models;

namespace DriverVerificationService.Repositories
{
    public interface IDriverRepository
    {
        Task<Driver> GetDriverAsync(string driverId);
        Task CreateOrUpdateDriverAsync(Driver driver);
        Task<IEnumerable<Driver>> GetPendingVerificationsAsync();
    }

}
