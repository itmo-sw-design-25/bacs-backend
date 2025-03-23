namespace BaCS.Domain.Core.ValueObjects;

public record ImageInfo
{
    public string FileName { get; init; }
    public long FileSize { get; init; }
    public string ContentType { get; init; }
    public Stream ImageData { get; init; }
}
