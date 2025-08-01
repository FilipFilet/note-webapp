using Backend_API.Models;
using Backend_API.Repositories;

namespace Backend_API.Services;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _folderRepository;

    public FolderService(IFolderRepository folderRepository)
    {
        _folderRepository = folderRepository;
    }

    public async Task AddFolderAsync(Folder folder)
    {
        await _folderRepository.AddFolderAsync(folder);
    }

    public async Task<updateFolderDTO> UpdateFolderAsync(int folderId, updateFolderDTO updateFolderDto)
    {
        var folder = await _folderRepository.GetFolderByIdAsync(folderId);

        folder.Name = updateFolderDto.Name;

        Folder updatedFolder = await _folderRepository.UpdateFolderAsync(folder);

        updateFolderDTO updatedFolderDto = new updateFolderDTO
        {
            Name = updatedFolder.Name,
        };
        return updatedFolderDto;
    }
}