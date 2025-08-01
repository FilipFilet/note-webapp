using Backend_API.Models;

public interface IUserService
{
    Task<GetUserDto?> GetUserByIdAsync(int id);
    Task<List<CreateUserDto>> GetUsersAsync();
}