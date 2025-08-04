using Backend_API.Models;
using Backend_API.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly INotesDBContext _context;
    public FolderRepository(INotesDBContext context)
    {
        _context = context;
    }

    public async Task AddFolderAsync(Folder folder)
    {
        await _context.Folders.AddAsync(folder);
        await _context.SaveChangesAsync();
    }

    public async Task<Folder> UpdateFolderAsync(Folder folder)
    {
        _context.Folders.Update(folder);
        await _context.SaveChangesAsync();
        return folder;
    }

    public async Task<Folder?> GetFolderByIdAsync(int folderId)
    {
        return await _context.Folders.FindAsync(folderId);
    }

    public async Task<List<Folder>> GetFoldersByUserIdAsync(int userId)
    {
        return await _context.Folders
            .Where(folder => folder.UserId == userId)
            .ToListAsync();
    }

    public async Task<List<Folder>> GetFoldersWithNotesByUserIdAsync(int userId)
    {
        return await _context.Folders
            .Include(folder => folder.Notes)
            .Where(folder => folder.UserId == userId)
            .ToListAsync();
    }
}