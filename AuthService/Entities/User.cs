using System.ComponentModel.DataAnnotations;

namespace AuthService.Entities;
public enum Roles
{
    Admin,
    User
};

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MinLength(6)]
    [MaxLength(25)]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string Name { get; set; } = String.Empty;
    public bool MailVerified { get; set; } = false;
    public Roles Role { get; set; } = Roles.User;
}
