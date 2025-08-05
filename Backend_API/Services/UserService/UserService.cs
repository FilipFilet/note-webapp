using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using static BCrypt.Net.BCrypt;



public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly INoteRepository _noteRepository;

    public UserService(IUserRepository userRepository, IFolderRepository folderRepository, INoteRepository noteRepository)
    {
        _userRepository = userRepository;
        _folderRepository = folderRepository;
        _noteRepository = noteRepository;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");


        await _userRepository.DeleteUserAsync(user);
    }

    public async Task<GetUserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null) throw new KeyNotFoundException("User not found");

        var retrievedUser = new GetUserDto
        {
            Username = user.Username
        };

        return retrievedUser;
    }

    public async Task<UserContentDTO> GetUserContentAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found");

        var folders = await _folderRepository.GetFoldersWithNotesByUserIdAsync(userId);
        var independentNotes = await _noteRepository.GetIndependentNotesByUserIdAsync(userId);

        // Converting raw data to DTOs
        var foldersDTO = folders.Select(folder => new GetFolderDto
        {
            Name = folder.Name,
            Notes = folder.Notes.Select(note => new GetNoteDto
            {
                Title = note.Title,
                Content = note.Content
            }).ToList()
        }).ToList();

        var IndependentNotesDTO = independentNotes.Select(note => new GetNoteDto
        {
            Title = note.Title,
            Content = note.Content,
        }).ToList();


        return new UserContentDTO
        {
            Folders = foldersDTO,
            Notes = IndependentNotesDTO
        };
    }

    public async Task<UpdateUserDTO?> UpdateUserAsync(int id, UpdateUserDTO updateUserDTO)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        var potentialOverlapUser = await _userRepository.GetUserByUsernameAsync(updateUserDTO.Username);

        if (user == null) throw new KeyNotFoundException("User not found");
        if (potentialOverlapUser != null) throw new ArgumentException("Username already exists");
        if (updateUserDTO.Username == user.Username) throw new ArgumentException("Username cant be the same as the current one");

        // Updating properties based on the DTO
        if (updateUserDTO.Username != null) user.Username = updateUserDTO.Username;

        // Call the repository to update the user
        var updatedUser = await _userRepository.UpdateUserAsync(user);

        return new UpdateUserDTO
        {
            Username = updatedUser.Username,
        };
    }

}