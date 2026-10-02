using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Infrastructure.FileStorage;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _client;
    private readonly MinioOptions _options;

    public MinioFileStorageService(
        IMinioClient client,
        IOptions<MinioOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task UploadAsync(
        Stream content,
        string objectKey,
        string contentType,
        CancellationToken cancellationToken)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var args = new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectKey)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType(contentType);

        await _client.PutObjectAsync(
            args,
            cancellationToken);
    }

    public async Task<Stream> DownloadAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectKey)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            });

        await _client.GetObjectAsync(
            args,
            cancellationToken);

        memoryStream.Position = 0;

        return memoryStream;
    }

    public async Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectKey);

        await _client.RemoveObjectAsync(
            args,
            cancellationToken);
    }

    private async Task EnsureBucketExistsAsync(
        CancellationToken cancellationToken)
    {
        var existsArgs = new BucketExistsArgs()
            .WithBucket(_options.BucketName);

        var exists = await _client.BucketExistsAsync(
            existsArgs,
            cancellationToken);

        if (exists)
        {
            return;
        }

        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(_options.BucketName);

        await _client.MakeBucketAsync(
            makeBucketArgs,
            cancellationToken);
    }
}