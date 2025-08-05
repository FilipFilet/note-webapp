using Backend_API.Models;

namespace Backend_API.Services;

public interface IFolderService
{
    Task<Folder> AddFolderAsync(CreateFolderDto createFolderDto, int userId);
    Task<updateFolderDTO> UpdateFolderAsync(int folderId, int userId, updateFolderDTO updateFolderDto);
    Task<Folder?> GetFolderByIdAsync(int folderId, int userId);
    Task DeleteFolderAsync(int folderId, int userId);

}