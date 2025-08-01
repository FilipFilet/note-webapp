using Backend_API.Models;
using Backend_API.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.Repositories;

public interface INoteRepository
{
    Task<Note> GetNoteByIdAsync(int id);
    Task<List<Note>> GetNotesAsync();
    Task<List<Note>> GetNotesByUserIdAsync(int userId);
    Task AddNoteAsync(Note note);
    Task UpdateNoteAsync(Note note);
    Task DeleteNoteAsync(int id);
    Task<List<Note>> GetIndependentNotesByUserIdAsync(int userId);
}