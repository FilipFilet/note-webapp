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

    public async Task<Folder?> GetFolderByIdAsync(int folderId, int userId)
    {
        var folder = await _folderRepository.GetFolderByIdAsync(folderId);

        if (folder == null) throw new KeyNotFoundException($"Folder with ID {folderId} not found.");
        if (folder.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to access this folder.");

        return folder;
    }

    public async Task<Folder> AddFolderAsync(CreateFolderDto createFolderDto, int userId)
    {
        Folder newFolder = new Folder
        {
            Name = createFolderDto.Name,
            UserId = userId
        };

        await _folderRepository.AddFolderAsync(newFolder);
        return newFolder;
    }

    public async Task<updateFolderDTO> UpdateFolderAsync(int folderId, int userId, updateFolderDTO updateFolderDto)
    {
        var folder = await _folderRepository.GetFolderByIdAsync(folderId);


        if (folder == null) throw new KeyNotFoundException($"Folder with ID {folderId} not found.");
        if (folder.Name == updateFolderDto.Name) throw new ArgumentException("New folder name cannot be the same as the current name.");
        if (folder.UserId != userId) throw new UnauthorizedAccessException("You do not have permission to update this folder.");


        folder.Name = updateFolderDto.Name;

        Folder updatedFolder = await _folderRepository.UpdateFolderAsync(folder);

        updateFolderDTO updatedFolderDto = new updateFolderDTO
        {
            Name = updatedFolder.Name,
        };
        return updatedFolderDto;
    }
}