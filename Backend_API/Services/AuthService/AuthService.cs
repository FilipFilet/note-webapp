using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace Backend_API.Services;

public class AuthService : IAuthService
{
    //
    const int BCryptSaltWorkFactor = 12; // Controls how many iterations of hashing are done. (2^12 = 4096 iterations)
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _config = config;
        _userRepository = userRepository;
    }

    public async Task<String> ValidateUserAsync(CreateUserDto userDto)
    {
        User? user = await _userRepository.GetUserByUsernameAsync(userDto.Username);

        // If user doesnt exist, or password is incorrect
        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        // Configure JWT Signature
        var signingCredentials = new SigningCredentials(
            //                       Converts the secret key from a string to a byte array, since SymmetricSecurityKey expects a byte array    
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256Signature
        );

        // Configure custom claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        // Create JWT token object with claims and signature
        var jwtObject = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: signingCredentials
        );

        // Serialize the JWT token to a string represntted as "header.payload.signature"
        var jwtString = new JwtSecurityTokenHandler().WriteToken(jwtObject);
        return jwtString;
    }

    public async Task<User> AddUserAsync(CreateUserDto userDto)
    {
        // Creates a new user object
        User user = new User
        {
            Username = userDto.Username,

            // Hashes the password with BCrypt
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password, BCryptSaltWorkFactor)
        };

        await _userRepository.AddUserAsync(user);
        return user;
    }

}