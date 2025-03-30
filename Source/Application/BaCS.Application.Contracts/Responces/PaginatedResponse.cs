namespace BaCS.Application.Contracts.Responces;

public record PaginatedResponse<T>
{
    public IReadOnlyCollection<T> Collection { get; init; }
    public int TotalCount { get; init; }
}
