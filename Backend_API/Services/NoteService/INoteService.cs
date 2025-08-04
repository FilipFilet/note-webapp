using Backend_API.Models;
using Microsoft.EntityFrameworkCore;
using Backend_API.Repositories;

namespace Backend_API.Services;

public interface INoteService
{
    Task<Note> GetNoteByIdAsync(int id, int userId);
    Task<Note> AddNoteAsync(CreateNoteDto noteDto, int userId);
    Task<UpdateNoteDTO> UpdateNoteAsync(int userid, int id, UpdateNoteDTO updateNoteDTO);
    Task DeleteNoteAsync(int id);
}