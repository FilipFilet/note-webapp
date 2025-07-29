using Backend_API.Models;

namespace Backend_API.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<List<User>> GetUsersAsync(string username);
    Task AddUserAsync(User user);
}
