using System.Security.Claims;
using Backend_API.Models;

namespace Backend_API.Services;

public interface IAuthService
{
    Task<AuthenticatedResponse> ValidateUserAsync(LoginUserDto userDto);
    Task<User> AddUserAsync(CreateUserDto userDto);
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
