using Backend_API.Models;
using Backend_API.DBContext;
using Microsoft.EntityFrameworkCore;
using Backend_API.Repositories;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;

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
        return await _context.Users
            .Where(user => user.Id == id)
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

    public async Task<User?> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}