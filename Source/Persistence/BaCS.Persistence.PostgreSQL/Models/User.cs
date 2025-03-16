namespace BaCS.Persistence.PostgreSQL.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
}
