namespace PropertyPro.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task UploadAsync(
        Stream content,
        string objectKey,
        string contentType,
        CancellationToken cancellationToken);

    Task<Stream> DownloadAsync(
        string objectKey,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken);
}