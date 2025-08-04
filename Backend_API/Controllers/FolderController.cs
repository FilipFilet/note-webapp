using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class FolderController : ControllerBase
{
    private readonly IFolderService _folderService;

    public FolderController(IFolderService folderService)
    {
        _folderService = folderService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFolderById(int folderId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            var folder = await _folderService.GetFolderByIdAsync(folderId, userId);
            return Ok(folder);
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddFolder(CreateFolderDto createFolderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var newFolder = await _folderService.AddFolderAsync(createFolderDto, userId);
        return CreatedAtAction(nameof(GetFolderById), new { id = newFolder.Id }, createFolderDto);
    }

    [Authorize]
    [HttpPut("{folderId}")]
    public async Task<IActionResult> UpdateFolder(int folderId, updateFolderDTO updateFolderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            var updatedFolder = await _folderService.UpdateFolderAsync(folderId, userId, updateFolderDto);
            return Ok(updatedFolder);
        }
        catch (KeyNotFoundException err)
        {
            return NotFound(err.Message);
        }
        catch (UnauthorizedAccessException err)
        {
            return Unauthorized(err.Message);
        }
        catch (ArgumentException err)
        {
            return BadRequest(err.Message);
        }
    }
}