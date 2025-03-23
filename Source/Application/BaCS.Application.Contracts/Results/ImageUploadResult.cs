namespace BaCS.Application.Contracts.Results;

public record ImageUploadResult
{
    public bool Success { get; init; }
    public string? Path { get; init; }
    public string? Error { get; init; }

    public static ImageUploadResult Ok(string path) => new() { Success = true, Path = path };
    public static ImageUploadResult Fail(string error) => new() { Success = false, Error = error };
}
