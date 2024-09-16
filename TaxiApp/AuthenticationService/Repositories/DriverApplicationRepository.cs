using AuthenticationService.Data;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repositories
{
    public class DriverApplicationRepository : IDriverApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetPendingDriverVerificationsAsync()
        {
            return await _context.Users
                .Where(u => u.UserType == UserType.Driver && u.VerificationStatus == VerificationStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateVerificationStatusAsync(User user, bool isApproved)
        {
            user.VerificationStatus = isApproved ? VerificationStatus.Approved : VerificationStatus.Rejected;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
