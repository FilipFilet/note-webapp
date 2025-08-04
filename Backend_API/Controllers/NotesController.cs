using Microsoft.AspNetCore.Mvc;
using Backend_API.Services;
using Backend_API.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Backend_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNote(CreateNoteDto noteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            var note = await _noteService.AddNoteAsync(noteDto, userId);

            var noteDTO = new CreateNoteDto
            {
                Title = note.Title,
                Content = note.Content,
                FolderId = note.FolderId,
            };

            return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, noteDTO);
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(int noteId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            Note note = await _noteService.GetNoteByIdAsync(noteId, userId);
            return Ok(note);
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
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, UpdateNoteDTO updateNoteDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        try
        {
            var updatedNote = await _noteService.UpdateNoteAsync(userId, id, updateNoteDTO);
            return Ok(updatedNote);
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
