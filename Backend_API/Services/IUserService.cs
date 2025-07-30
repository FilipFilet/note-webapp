using Backend_API.Models;

public interface IUserService
{
    Task AddUserAsync(User user);
    Task<GetUserDto?> GetUserByIdAsync(int id);
    Task<List<CreateUserDto>> GetUsersAsync();
}