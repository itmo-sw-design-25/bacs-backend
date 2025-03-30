namespace BaCS.Domain.Core.Entities;

using Abstractions;
using Enums;

public class Resource : AuditableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid LocationId { get; init; }

    public string Name { get; set; }
    public string Description { get; set; }
    public int Floor { get; set; }
    public string[] Equipment { get; set; }
    public ResourceType Type { get; set; }
    public string ImageUrl { get; set; }

    public virtual Location Location { get; init; }
}
