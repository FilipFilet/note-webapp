using Backend_API.Models;

public interface IUserService
{
    Task<User> AddUserAsync(CreateUserDto userDto);
    Task<GetUserDto?> GetUserByIdAsync(int id);

    Task<String> ValidateUserAsync(CreateUserDto userDto);
    Task<List<CreateUserDto>> GetUsersAsync();
}