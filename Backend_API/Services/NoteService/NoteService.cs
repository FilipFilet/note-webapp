using Backend_API.Models;
using Microsoft.EntityFrameworkCore;
using Backend_API.Repositories;

namespace Backend_API.Services;

public class NoteService : INoteService
{
    INoteRepository _noteRepository;
    IFolderRepository _folderRepository;

    public NoteService(INoteRepository noteRepository, IFolderRepository folderRepository)
    {
        _noteRepository = noteRepository;
        _folderRepository = folderRepository;
    }

    public async Task<Note> AddNoteAsync(CreateNoteDto noteDto, int userId)
    {
        if (noteDto.FolderId.HasValue && noteDto.FolderId.Value > 0)
        {
            var existingFolder = await _folderRepository.GetFolderByIdAsync(noteDto.FolderId.Value);

            if (existingFolder == null) throw new KeyNotFoundException($"Folder with ID {noteDto.FolderId} not found.");
            if (existingFolder.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to add notes to this folder.");

        }

        Note note = new Note
        {
            Title = noteDto.Title,
            Content = noteDto.Content,
            UserId = userId,
            FolderId = noteDto.FolderId <= 0 ? null : noteDto.FolderId
        };

        await _noteRepository.AddNoteAsync(note);
        return note;
    }

    public Task DeleteNoteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Note> GetNoteByIdAsync(int id, int userId)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);

        if (note == null) throw new KeyNotFoundException($"Note with ID {id} not found.");
        if (note.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to access this note.");

        return note;
    }

    public async Task<UpdateNoteDTO> UpdateNoteAsync(int userid, int id, UpdateNoteDTO updateNoteDTO)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);

        if (note == null) throw new KeyNotFoundException($"Note with ID {id} not found.");
        else if (note.UserId != userid) throw new UnauthorizedAccessException("You do not have permission to update this note.");

        // No changes made to the note so no ressources used to update it
        // Return the original note as a DTO
        if (note.Title == updateNoteDTO.Title && note.Content == updateNoteDTO.Content)
        {
            return new UpdateNoteDTO
            {
                Title = note.Title,
                Content = note.Content
            };
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