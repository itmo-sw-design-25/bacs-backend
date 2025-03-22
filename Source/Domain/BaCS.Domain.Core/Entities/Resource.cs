namespace BaCS.Domain.Core.Entities;

using Abstractions;
using Enums;

public class Resource : UpdatableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid LocationId { get; init; }

    public string Name { get; init; }
    public string Description { get; init; }
    public int Floor { get; init; }
    public string[] Equipment { get; init; }
    public ResourceType Type { get; init; }

    public virtual Location Location { get; init; }
}
