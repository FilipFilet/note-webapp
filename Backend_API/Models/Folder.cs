using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

// Representation of folder entity in the database
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

// DTO for creating a folder
public class CreateFolderDto
{
    [Required]
    public string Name { get; set; }
}

// DTO for updating a folder
public class updateFolderDTO
{
    [Required]
    public string Name { get; set; }
}

// DTO for retrieving folder information
public class GetFolderDto
{
    public string Name { get; set; }
    public List<GetNoteDto> Notes { get; set; } = new List<GetNoteDto>();
}
