using AuthenticationService.Models;

namespace AuthenticationService.Repositories
{
    public interface IDriverApplicationRepository
    {
        Task<IEnumerable<User>> GetPendingDriverVerificationsAsync();
        Task UpdateVerificationStatusAsync(User user, bool isApproved);
    }
}
