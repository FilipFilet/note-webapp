using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

// Representation of user entity in the database
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

// DTO for creating a user
public class CreateUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}

// DTO for retrieving user information
public class GetUserDto
{
    public string Username { get; set; }
}

// DTO for updating user information
public class UpdateUserDTO
{
    [Required]
    public string Username { get; set; }
    // Possibly profile picture, email, etc.

}
