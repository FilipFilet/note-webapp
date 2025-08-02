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

    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        List<Note> notes = await _noteService.GetNotesAsync();
        return Ok(notes);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddNote(CreateNoteDto noteDto)
    {
        Note note = new Note
        {
            Title = noteDto.Title,
            Content = noteDto.Content,
            UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
            FolderId = noteDto.FolderId
        };

        await _noteService.AddNoteAsync(note);
        return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, note);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById(int id)
    {
        Note note = await _noteService.GetNoteByIdAsync(id);
        return Ok(note);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNote(int id, UpdateNoteDTO updateNoteDTO)
    {
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
