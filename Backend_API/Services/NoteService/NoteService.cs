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
        // If the folderId in the dto has a value and is above 0
        if (noteDto.FolderId.HasValue && noteDto.FolderId.Value > 0)
        {
            var existingFolder = await _folderRepository.GetFolderByIdAsync(noteDto.FolderId.Value);

            // If no folder could be found with that id
            if (existingFolder == null) throw new KeyNotFoundException($"Folder with ID {noteDto.FolderId} not found.");

            // if UserId of the folder is different than the one in the JWT
            if (existingFolder.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to add notes to this folder.");

        }

        Note note = new Note
        {
            Title = noteDto.Title,
            Content = noteDto.Content,
            UserId = userId,

            // If incoming folderid is <= 0, set to null, else the incoming id
            FolderId = noteDto.FolderId <= 0 ? null : noteDto.FolderId
        };

        await _noteRepository.AddNoteAsync(note);
        return note;
    }

    public async Task DeleteNoteAsync(int id, int userId)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);

        if (note == null) throw new KeyNotFoundException($"Note with ID {id} not found.");
        if (note.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to delete this note.");

        await _noteRepository.DeleteNoteAsync(note);
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
        // Get note to be updated
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

        // Set new values
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

    public async Task<UpdateNoteFolderDTO> UpdateNoteFolderAsync(int userId, int id, UpdateNoteFolderDTO updateNoteFolderDTO)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);

        if (note == null) throw new KeyNotFoundException($"Note with ID {id} not found.");
        if (note.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to update this note's folder.");

        note.FolderId = updateNoteFolderDTO.FolderId <= 0 ? null : updateNoteFolderDTO.FolderId;

        var updatedNote = await _noteRepository.UpdateNoteAsync(note);

        return new UpdateNoteFolderDTO
        {
            FolderId = updatedNote.FolderId
        };
    }


}