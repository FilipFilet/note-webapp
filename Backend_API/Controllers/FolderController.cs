using Backend_API.Models;
using Backend_API.Services;
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

    [HttpPost]
    public async Task<IActionResult> AddFolder(CreateFolderDto createFolderDto)
    {
        Folder newFolder = new Folder
        {
            Name = createFolderDto.Name,
            UserId = createFolderDto.UserId
        };

        await _folderService.AddFolderAsync(newFolder);
        return CreatedAtAction(nameof(AddFolder), new { id = newFolder.Id }, createFolderDto);
    }

    [HttpPut("{folderId}")]
    public async Task<IActionResult> UpdateFolder(int folderId, updateFolderDTO updateFolderDto)
    {
        var updatedFolder = await _folderService.UpdateFolderAsync(folderId, updateFolderDto);
        return Ok(updatedFolder);
    }
}