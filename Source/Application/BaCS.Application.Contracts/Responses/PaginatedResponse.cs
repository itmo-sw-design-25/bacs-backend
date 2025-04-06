namespace BaCS.Application.Contracts.Responses;

using System.ComponentModel;

public record PaginatedResponse<T>
{
    [Description("Коллекция элементов.")]
    public IReadOnlyCollection<T> Collection { get; init; }

    [Description("Общее количество элементов в коллекции.")]
    public int TotalCount { get; init; }
}
