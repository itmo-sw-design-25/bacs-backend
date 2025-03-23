namespace BaCS.Domain.Core.Abstractions;

public abstract class UpdatableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
