namespace OrderService.Entities;

public class User
{
    public int Id { get; set; }
    public int OriginalUserId { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
}
