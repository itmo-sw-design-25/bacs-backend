namespace BaCS.Domain.Core.Entities;

using Abstractions;

public class Location : AuditableEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public virtual CalendarSettings CalendarSettings { get; set; }
    public virtual ICollection<Resource> Resources { get; init; } = [];
    public virtual ICollection<User> Admins { get; init; } = [];
}
