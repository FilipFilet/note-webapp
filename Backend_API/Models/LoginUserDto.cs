using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models;

public class LoginUserDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}