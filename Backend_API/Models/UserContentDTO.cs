using Backend_API.Models;
namespace Backend_API.Models;

public class UserContentDTO
{
    public List<GetFolderDto> Folders { get; set; }
    public List<GetNoteDto> Notes { get; set; }
}
