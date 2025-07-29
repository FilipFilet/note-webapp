using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

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

public class CreateNoteDto
{
    [Required]
    public string Title { get; set; }
    public string? Content { get; set; }
    public int? FolderId { get; set; }
    [Required]
    public int UserId { get; set; }
}