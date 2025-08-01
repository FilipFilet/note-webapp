using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Repositories;
using Microsoft.IdentityModel.Tokens;
using static BCrypt.Net.BCrypt;



public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserDto?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<List<CreateUserDto>> GetUsersAsync()
    {
        return await _userRepository.GetUsersAsync();
    }
}