using Backend_API.Models;

namespace Backend_API.Repositories;

public interface IFolderRepository
{
    Task AddFolderAsync(Folder folder);
    Task<Folder> UpdateFolderAsync(Folder folder);
    Task<Folder?> GetFolderByIdAsync(int folderId);

    Task<List<Folder>> GetFoldersByUserIdAsync(int userId);

    Task<List<Folder>> GetFoldersWithNotesByUserIdAsync(int userId);

    Task DeleteFolderAsync(Folder folder);

}
