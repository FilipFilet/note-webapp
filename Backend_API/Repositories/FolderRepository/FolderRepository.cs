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

    // Should realistically only retrieve the data, not do a conversion to DTO here.
    // This should be handled in the service layer or controller.
    public async Task<List<Folder>> GetFoldersByUserIdAsync(int userId)
    {
        return await _context.Folders
            .Where(folder => folder.UserId == userId)
            .ToListAsync();
    }
}