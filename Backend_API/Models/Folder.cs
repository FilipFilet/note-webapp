using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

public class Folder
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    public List<Note> Notes { get; set; } = new List<Note>();
}

public class GetFolderDto
{
    public string Name { get; set; }
    public List<GetNoteDto> Notes { get; set; } = new List<GetNoteDto>();
}
