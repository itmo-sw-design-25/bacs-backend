namespace BaCS.Domain.Core.Entities;

using Abstractions;

public class User : AuditableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public string Email { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public bool EnableEmailNotifications { get; set; } = true;
}
