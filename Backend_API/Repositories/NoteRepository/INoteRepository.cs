using Backend_API.Models;
using Backend_API.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.Repositories;

public interface INoteRepository
{
    Task<Note?> GetNoteByIdAsync(int id);
    Task<List<Note>> GetNotesAsync();
    Task AddNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(Note note);
    Task<List<Note>> GetIndependentNotesByUserIdAsync(int userId);
}