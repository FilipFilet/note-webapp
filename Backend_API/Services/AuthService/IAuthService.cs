using Backend_API.Models;

namespace Backend_API.Services;

public interface IAuthService
{
    Task<string> ValidateUserAsync(CreateUserDto userDto);
    Task<User> AddUserAsync(CreateUserDto userDto);
}
