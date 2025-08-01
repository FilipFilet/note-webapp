using Backend_API.Models;
using Backend_API.DBContext;
using Microsoft.EntityFrameworkCore;
using Backend_API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly INotesDBContext _context;

    public UserRepository(INotesDBContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<GetUserDto?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Where(user => user.Id == id)
            .Select(user => new GetUserDto
            {
                Username = user.Username,
                Notes = user.Notes.Select(note => new GetNoteDto
                {
                    Title = note.Title,
                    Content = note.Content
                }).ToList(),
                Folders = user.Folders.Select(folder => new GetFolderDto
                {
                    Name = folder.Name,
                    Notes = folder.Notes.Select(note => new GetNoteDto
                    {
                        Title = note.Title,
                        Content = note.Content
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CreateUserDto>> GetUsersAsync()
    {
        return await _context.Users
            .Select(user => new CreateUserDto
            {
                Username = user.Username,
                Password = user.Password
            })
            .ToListAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user => user.Username == username);
    }
}