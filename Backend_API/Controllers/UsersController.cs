using Microsoft.AspNetCore.Mvc;
using Backend_API.Services;
using Backend_API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetUserById()
    {

        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            GetUserDto? user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (KeyNotFoundException err)
        {
            return NotFound(err.Message);
        }
    }

    [Authorize]
    [HttpGet("me/content")]
    public async Task<IActionResult> GetMyContent()
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        try
        {
            UserContentDTO content = await _userService.GetUserContentAsync(int.Parse(userId));
            return Ok(content);
        }
        catch (KeyNotFoundException err)
        {
            return NotFound(err.Message);
        }
    }

    [Authorize]
    [HttpPut("UpdateUserInfo")]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO updateUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            var updatedUserDTO = await _userService.UpdateUserAsync(userId, updateUserDTO);
            return Ok(updatedUserDTO);
        }
        catch (KeyNotFoundException err)
        {
            return NotFound(err.Message);
        }
        catch (ArgumentException err)
        {
            return BadRequest(err.Message);
        }
        catch (NullReferenceException err)
        {
            return BadRequest(err.Message);
        }
    }

    [Authorize]
    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
        catch (KeyNotFoundException err)
        {
            return NotFound(err.Message);
        }
        catch (UnauthorizedAccessException err)
        {
            return Unauthorized(err.Message);
        }
    }

}