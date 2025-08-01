using Backend_API.Models;
using Backend_API.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly INotesDBContext _context;

    public NoteRepository(INotesDBContext context)
    {
        _context = context;
    }

    public async Task AddNoteAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteNoteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Note> GetNoteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Note>> GetNotesAsync()
    {
        return await _context.Notes.ToListAsync();
    }

    public async Task<List<Note>> GetNotesByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateNoteAsync(Note note)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Note>> GetIndependentNotesByUserIdAsync(int userId)
    {
        return await _context.Notes
            .Where(note => note.UserId == userId && note.FolderId == null)
            .ToListAsync();
    }
}