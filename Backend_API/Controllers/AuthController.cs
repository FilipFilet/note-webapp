using Backend_API.Models;
using Backend_API.Services;
using Microsoft.AspNetCore.Mvc;


namespace Backend_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CreateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var jwtString = await _authService.ValidateUserAsync(userDto);
            return Ok(jwtString);
        }
        catch (UnauthorizedAccessException err)
        {
            return Unauthorized(err.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> AddUser(CreateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _authService.AddUserAsync(userDto);
        return Ok("User created successfully");
    }
}