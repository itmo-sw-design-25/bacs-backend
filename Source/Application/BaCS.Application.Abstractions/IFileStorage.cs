namespace BaCS.Application.Abstractions;

using Contracts.Results;
using Domain.Core.ValueObjects;

public interface IFileStorage
{
    Task<ImageUploadResult> UploadImage(
        ImageInfo imageInfo,
        string bucket,
        CancellationToken cancellationToken = default
    );

    Task<ImageUploadResult> DeleteImage(
        string fileName,
        string bucket,
        CancellationToken cancellationToken = default
    );
}
