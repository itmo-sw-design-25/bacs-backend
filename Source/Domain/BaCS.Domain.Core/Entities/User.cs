namespace BaCS.Domain.Core.Entities;

using Abstractions;

public class User : AuditableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public string Username { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}
