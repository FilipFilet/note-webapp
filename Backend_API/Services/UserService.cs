using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Repositories;
using Microsoft.IdentityModel.Tokens;
using static BCrypt.Net.BCrypt;



public class UserService : IUserService
{
    const int BCryptSaltWorkFactor = 12;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public UserService(IUserRepository userRepository, IConfiguration config)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public async Task<User> AddUserAsync(CreateUserDto userDto)
    {
        User user = new User
        {
            Username = userDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password, BCryptSaltWorkFactor)
        };

        // Add try catch
        await _userRepository.AddUserAsync(user);
        return user;
    }

    public async Task<String> ValidateUserAsync(CreateUserDto userDto)
    {
        User? user = await _userRepository.GetUserByUsernameAsync(userDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256Signature
        );

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var jwtObject = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: signingCredentials
        );

        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwtObject);
        return jwtString;
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