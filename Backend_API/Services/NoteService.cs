using Backend_API.Models;
using Microsoft.EntityFrameworkCore;
using Backend_API.Repositories;

namespace Backend_API.Services;

public class NoteService : INoteService
{
    INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task AddNoteAsync(Note note)
    {
        await _noteRepository.AddNoteAsync(note);
    }

    public Task DeleteNoteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Note> GetNoteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Note>> GetNotesAsync()
    {
        return await _noteRepository.GetNotesAsync();
    }

    public async Task<List<Note>> GetNotesByFolderIdAsync(int folderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Note>> GetNotesByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNoteAsync(Note note)
    {
        throw new NotImplementedException();
    }
}