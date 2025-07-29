using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }

    public List<Folder> Folders { get; set; } = new List<Folder>();
    public List<Note> Notes { get; set; } = new List<Note>();
}

public class CreateUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}
