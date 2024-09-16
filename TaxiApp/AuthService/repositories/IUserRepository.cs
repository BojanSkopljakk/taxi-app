using AuthService.Models;

namespace AuthService.repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> RegisterUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);

        Task<User> LoginAsync(string email, string password);

        Task<List<User>> GetAllUsersAsync();

    }
}
