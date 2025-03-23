namespace BaCS.Persistence.Minio.Services;

using System.Net;
using Application.Abstractions;
using Application.Contracts.Results;
using Domain.Core.ValueObjects;
using global::Minio;
using global::Minio.DataModel.Args;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options;

public class MinioFileStorage(
    IMinioClient minioClient,
    IOptions<MinioOptions> minioOptions,
    ILogger<MinioFileStorage> logger
) : IFileStorage
{
    private const string DefaultBucketPolicy =
        """
        {
          "Version": "2012-10-17",
          "Statement": [
            {
              "Effect": "Allow",
              "Principal": { "AWS": ["*"] },
              "Action": [ "s3:GetObject" ],
              "Resource": [ "arn:aws:s3:::{bucket}/*" ]
            }
          ]
        }
        """;

    private readonly MinioOptions _minioOptions = minioOptions.Value;

    public async Task<ImageUploadResult> UploadImage(
        ImageInfo imageInfo,
        string bucket,
        CancellationToken cancellationToken
    )
    {
        await EnsureBucketExists(bucket, cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(imageInfo.FileName)
            .WithObjectSize(imageInfo.FileSize)
            .WithStreamData(imageInfo.ImageData)
            .WithContentType(imageInfo.ContentType);

        try
        {
            var res = await minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

            if (res.ResponseStatusCode is not HttpStatusCode.OK)
            {
                logger.LogWarning(
                    "Failed to upload image to bucket {Bucket} with status code {StatusCode}",
                    imageInfo.FileName,
                    res.ResponseStatusCode
                );

                return ImageUploadResult.Fail(res.ResponseContent);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to upload image {FileName} to bucket {Bucket}", imageInfo.FileName, bucket);

            return ImageUploadResult.Fail(ex.Message);
        }

        return ImageUploadResult.Ok($"{_minioOptions.ProxyUrl}/{bucket}/{imageInfo.FileName}");
    }

    public async Task<ImageUploadResult> DeleteImage(
        string fileName,
        string bucket,
        CancellationToken cancellationToken = default
    )
    {
        await EnsureBucketExists(bucket, cancellationToken);

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);

        try
        {
            await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete image {FileName} from bucket {Bucket}", fileName, bucket);

            return ImageUploadResult.Fail(ex.Message);
        }

        return ImageUploadResult.Ok($"{_minioOptions.ProxyUrl}/{bucket}/{fileName}");
    }

    private async Task EnsureBucketExists(string bucket, CancellationToken cancellationToken)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucket);
        var bucketExists = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

        if (bucketExists) return;

        logger.LogWarning("Bucket {Bucket} doesn't exist to upload image", bucket);

        var makeBucketArgs = new MakeBucketArgs().WithBucket(bucket);
        await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);

        var setPolicyArgs = new SetPolicyArgs()
            .WithBucket(bucket)
            .WithPolicy(DefaultBucketPolicy.Replace("{bucket}", bucket));

        await minioClient.SetPolicyAsync(setPolicyArgs, cancellationToken);

        logger.LogInformation("Created bucket {Bucket} with default policy", bucket);
    }
}
