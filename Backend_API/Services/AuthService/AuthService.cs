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

    public async Task<AuthenticatedResponse> ValidateUserAsync(LoginUserDto userDto)
    {
        User? user = await _userRepository.GetUserByUsernameAsync(userDto.Username);

        // If user doesnt exist, or password is incorrect. The verify is case sensitive.
        if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var jwtString = GenerateAccessToken(claims);

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = BCrypt.Net.BCrypt.HashPassword(refreshToken, BCryptSaltWorkFactor); // Hash the refresh token before storing
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Example: 7 days validity
        await _userRepository.UpdateUserAsync(user);

        return new AuthenticatedResponse
        {
            AccessToken = jwtString,
            RefreshToken = refreshToken
        };
    }

    public async Task<User> AddUserAsync(CreateUserDto userDto)
    {
        // Check if username is already taken
        User? existingUser = await _userRepository.GetUserByUsernameAsync(userDto.Username);
        if (existingUser != null)
        {
            throw new ArgumentException("Username is already taken.");
        }

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

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        // Configure JWT Signature
        var signingCredentials = new SigningCredentials(
            //                       Converts the secret key from a string to a byte array, since SymmetricSecurityKey expects a byte array    
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256Signature
        );

        // Configure custom claims


        // Create JWT token object with claims and signature
        var jwtObject = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: signingCredentials
        );

        // Serialize the JWT token to a string represented as "header.payload.signature"
        return new JwtSecurityTokenHandler().WriteToken(jwtObject);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            ValidateLifetime = false, // We want to get claims from expired tokens as well
            ValidIssuer = _config["Jwt:Issuer"],
            ValidAudience = _config["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

}