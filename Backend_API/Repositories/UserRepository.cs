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

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetUsersAsync(string username)
    {
        return await _context.Users.ToListAsync();
    }
}