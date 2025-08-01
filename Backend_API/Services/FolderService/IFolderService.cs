using Backend_API.Models;

namespace Backend_API.Services;

public interface IFolderService
{
    Task AddFolderAsync(Folder folder);
    Task<updateFolderDTO> UpdateFolderAsync(int folderId, updateFolderDTO updateFolderDto);
}