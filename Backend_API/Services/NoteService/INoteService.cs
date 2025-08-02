using Backend_API.Models;
using Microsoft.EntityFrameworkCore;
using Backend_API.Repositories;

namespace Backend_API.Services;

public interface INoteService
{
    Task<Note> GetNoteByIdAsync(int id);
    Task<List<Note>> GetNotesAsync();
    Task<List<Note>> GetNotesByUserIdAsync(int userId);
    Task<List<Note>> GetNotesByFolderIdAsync(int folderId);
    Task AddNoteAsync(Note note);
    Task<UpdateNoteDTO> UpdateNoteAsync(int userid, int id, UpdateNoteDTO updateNoteDTO);
    Task DeleteNoteAsync(int id);
}