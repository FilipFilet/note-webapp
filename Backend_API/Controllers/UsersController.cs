using Microsoft.AspNetCore.Mvc;
using Backend_API.Services;
using Backend_API.Models;
using System.Security.Claims;

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

    [HttpGet("me/content")]
    public async Task<IActionResult> GetMyContent()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        UserContentDTO content = await _userService.GetUserContentAsync(int.Parse(userId));
        return Ok(content);
    }
}