namespace BaCS.Persistence.Minio.Extensions;

using Application.Abstractions;
using global::Minio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Options;
using Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMinioStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var minioOptions = configuration.GetSection(nameof(MinioOptions)).Get<MinioOptions>()!;

        services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));

        services.AddMinio(
            minioClient => minioClient
                .WithEndpoint(new Uri(minioOptions.Url))
                .WithSSL(minioOptions.WithSSL)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
        );

        services.AddSingleton<IFileStorage, MinioFileStorage>();

        return services;
    }
}
