using Microsoft.AspNetCore.Mvc;
using Backend_API.Services;
using Backend_API.Models;

namespace Backend_API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(CreateUserDto userDto)
    {
        var jwtString = await _userService.ValidateUserAsync(userDto);
        return Ok(jwtString);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        List<CreateUserDto> users = await _userService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsersById(int id)
    {
        GetUserDto user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(CreateUserDto userDto)
    {
        var createdUser = await _userService.AddUserAsync(userDto);
        return CreatedAtAction(nameof(GetUsersById), new { id = createdUser.Id }, createdUser);
    }
}