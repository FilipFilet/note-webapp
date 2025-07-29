using Microsoft.AspNetCore.Mvc;
using Backend_API.Services;
using Backend_API.Models;
using System.Threading.Tasks;

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

    [HttpPost]
    public async Task<IActionResult> AddNote(CreateNoteDto noteDto)
    {
        Note note = new Note
        {
            Title = noteDto.Title,
            Content = noteDto.Content,
            UserId = noteDto.UserId,
            FolderId = noteDto.FolderId
        };

        await _noteService.AddNoteAsync(note);
        return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, note);
    }
}
