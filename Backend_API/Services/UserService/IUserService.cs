using Backend_API.Models;

public interface IUserService
{
    Task<GetUserDto?> GetUserByIdAsync(int id);
    Task<UserContentDTO> GetUserContentAsync(int userId);
    Task<UpdateUserDTO?> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO);
}