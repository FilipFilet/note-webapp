using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

// Representation of note entity in the database
public class Note
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }
    public string? Content { get; set; }
    public int? FolderId { get; set; }
    public Folder? Folder { get; set; }

    [Required]
    public int UserId { get; set; }

    public User User { get; set; }
}

// DTO for creating a note
public class CreateNoteDto
{
    [Required]
    public string Title { get; set; }
    public string? Content { get; set; }
    [Required]
    public int? FolderId { get; set; }
}

// DTO for retrieving note information
public class GetNoteDto
{
    public string Title { get; set; }
    public string? Content { get; set; }
}

// DTO for updating a note
public class UpdateNoteDTO
{
    [Required]
    public string Title { get; set; }
    public string? Content { get; set; }
}

// DTO for updating a which folder a note belongs to
public class UpdateNoteFolderDTO
{
    public int? FolderId { get; set; }
}