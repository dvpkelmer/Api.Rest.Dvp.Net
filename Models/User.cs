namespace apiBackend.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Username { get; set; } = null!;
}
