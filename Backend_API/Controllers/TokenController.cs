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
        if (tokenApiModel == null) return BadRequest("Invalid client request");


        string accessToken = tokenApiModel.AccessToken;
        var refreshToken = Request.Cookies["refreshToken"];

        var principal = _authService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; // this is mapped to the Name claim by default

        // Retrieve the user by username from your data source
        // This example assumes you have a method GetUserByUsernameAsync
        User? user = await _userRepository.GetUserByUsernameAsync(username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(refreshToken, user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return Unauthorized("Invalid refresh token");
        }

        var newAccessToken = _authService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _authService.GenerateRefreshToken();

        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(7)
        });

        user.RefreshToken = newRefreshToken;
        await _userRepository.UpdateUserAsync(user);

        return Ok(newAccessToken);

    }

    [HttpPost("revoke"), Authorize]
    public async Task<IActionResult> Revoke()
    {
        var username = User.Identity.Name;
        User? user = await _userRepository.GetUserByUsernameAsync(username);
        if (user is null) return BadRequest();

        user.RefreshToken = null;
        await _userRepository.UpdateUserAsync(user);

        return NoContent();
    }
}