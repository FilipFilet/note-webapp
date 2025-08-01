using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend_API.Models;
using Backend_API.Repositories;
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

    public async Task<GetUserDto?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<List<CreateUserDto>> GetUsersAsync()
    {
        return await _userRepository.GetUsersAsync();
    }

    public async Task<UserContentDTO> GetUserContentAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        var folders = await _folderRepository.GetFoldersByUserIdAsync(userId);
        var independentNotes = await _noteRepository.GetIndependentNotesByUserIdAsync(userId);

        var IndependentNotesDTO = independentNotes.Select(note => new GetNoteDto
        {
            Title = note.Title,
            Content = note.Content,
        }).ToList();


        return new UserContentDTO
        {
            Folders = folders,
            Notes = IndependentNotesDTO
        };
    }
}