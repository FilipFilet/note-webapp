using Backend_API.Models;

namespace Backend_API.Repositories;

public interface IFolderRepository
{
    Task AddFolderAsync(Folder folder);
    Task<Folder> UpdateFolderAsync(Folder folder);

    Task<Folder?> GetFolderByIdAsync(int folderId);

}
