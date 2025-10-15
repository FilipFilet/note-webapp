using Backend_API.DBContext;
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
    public async Task<IActionResult> Login(LoginUserDto userDto)
    {
        // Validates the annotations set in the CreateUserDto model
        // If the model is not valid, return error 400
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var authResponse = await _authService.ValidateUserAsync(userDto);




            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException err)
        {
            return Unauthorized(err.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> AddUser(CreateUserDto userDto)
    {
        // Validates the annotations set in the CreateUserDto model
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _authService.AddUserAsync(userDto);
        }
        catch (ArgumentException err)
        {
            return BadRequest(err.Message);
        }

        return Ok("User created successfully");
    }
}