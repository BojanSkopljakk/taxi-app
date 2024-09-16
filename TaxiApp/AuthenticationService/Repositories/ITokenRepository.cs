using Microsoft.AspNetCore.Identity;
using AuthenticationService.Models;

namespace AuthenticationService.Repositories
{
    public interface ITokenRepository
    {
        string GenerateToken(User user, IList<string> roles);
    }
}
