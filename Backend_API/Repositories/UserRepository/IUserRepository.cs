using Backend_API.Models;

namespace Backend_API.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<List<CreateUserDto>> GetUsersAsync();
    Task AddUserAsync(User user);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> UpdateUserAsync(User user);
}
