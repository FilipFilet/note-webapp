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

public class GetUserDto
{
    public string Username { get; set; }
}

public class UpdateUserDTO
{
    [Required]
    public string Username { get; set; }
    // Possibly profile picture, email, etc.

}
