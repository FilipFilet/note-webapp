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

    public async Task<UpdateNoteDTO> UpdateNoteAsync(int userid, int id, UpdateNoteDTO updateNoteDTO)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);

        if (note == null)
        {
            throw new KeyNotFoundException($"Note with ID {id} not found.");
        }
        else if (note.UserId != userid)
        {
            throw new UnauthorizedAccessException("You do not have permission to update this note.");
        }

        note.Title = updateNoteDTO.Title;
        note.Content = updateNoteDTO.Content;

        var updatedNote = await _noteRepository.UpdateNoteAsync(note);

        // Return the updated note as a DTO
        return new UpdateNoteDTO
        {
            Title = updatedNote.Title,
            Content = updatedNote.Content
        };
    }
}