namespace BaCS.Domain.Core.Entities;

using Abstractions;

public class User : UpdatableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public string Username { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
}
