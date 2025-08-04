using Backend_API.Models;

namespace Backend_API.Services;

public interface IFolderService
{
    Task<Folder> AddFolderAsync(CreateFolderDto createFolderDto, int userId);
    Task<updateFolderDTO> UpdateFolderAsync(int folderId, int userId, updateFolderDTO updateFolderDto);
    public Task<Folder?> GetFolderByIdAsync(int folderId, int userId);
}