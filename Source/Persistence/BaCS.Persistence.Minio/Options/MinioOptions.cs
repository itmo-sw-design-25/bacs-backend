namespace BaCS.Persistence.Minio.Options;

public class MinioOptions
{
    public string Url { get; init; }
    public string ProxyUrl { get; init; }
    public bool WithSSL { get; init; }
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
}
