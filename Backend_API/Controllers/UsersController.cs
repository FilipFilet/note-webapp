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
        User user = new User
        {
            Username = userDto.Username,
            Password = userDto.Password // Ensure password is hashed in the repository/service
        };

        await _userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUsersById), new { id = user.Id }, user);
    }
}