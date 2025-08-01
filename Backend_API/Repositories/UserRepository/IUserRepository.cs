using Backend_API.Models;

namespace Backend_API.Repositories;

public interface IUserRepository
{
    Task<GetUserDto?> GetUserByIdAsync(int id);
    Task<List<CreateUserDto>> GetUsersAsync();
    Task AddUserAsync(User user);

    Task<User?> GetUserByUsernameAsync(string username);
}
