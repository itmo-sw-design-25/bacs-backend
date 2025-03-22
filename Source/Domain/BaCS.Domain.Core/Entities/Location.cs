namespace BaCS.Domain.Core.Entities;

using Abstractions;

public class Location : UpdatableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public string Name { get; init; }
    public string Address { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }

    public virtual CalendarSettings CalendarSettings { get; init; }
    public virtual ICollection<Resource> Resources { get; init; } = [];
    public virtual ICollection<User> Admins { get; init; } = [];
}
