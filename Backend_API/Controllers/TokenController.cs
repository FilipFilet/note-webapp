using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Repositories;
using Backend_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TokenController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public TokenController(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
    {
        if (tokenApiModel.AccessToken == null) return BadRequest("Access Token is required");
        if (tokenApiModel.RefreshToken == null) return BadRequest("Refresh Token is required");


        string accessToken = tokenApiModel.AccessToken;
        var refreshToken = tokenApiModel.RefreshToken;

        if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Refresh Token is empty");
        if (string.IsNullOrEmpty(accessToken)) return BadRequest("Access Token is empty");

        var principal = _authService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; // this is mapped to the Name claim by default


        User? user = await _userRepository.GetUserByUsernameAsync(username);

        // Validates the refresh token
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return Unauthorized("Unable to verify refresh token");
        }

        // Generates a new access token
        var newAccessToken = _authService.GenerateAccessToken(principal.Claims);

        // Generates a new refresh token and updates the cookie and database
        var newRefreshToken = _authService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userRepository.UpdateUserAsync(user);

        return Ok(new AuthenticatedResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });

    }

    [HttpPost("revoke"), Authorize]
    public async Task<IActionResult> Revoke()
    {
        var username = User.Identity.Name;
        User? user = await _userRepository.GetUserByUsernameAsync(username);
        if (user == null) return BadRequest();

        user.RefreshToken = null;
        await _userRepository.UpdateUserAsync(user);

        return NoContent();
    }
}