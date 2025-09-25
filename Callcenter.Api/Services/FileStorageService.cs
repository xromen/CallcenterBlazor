using Callcenter.Api.Models.Exceptions;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace Callcenter.Api.Services;

public class FileStorageService(IMinioClient minioClient)
{
    private const string BucketName = "callcenter";
    public async Task UploadFileAsync(string objectName, Stream fileStream, CancellationToken cancellationToken)
    {
        bool exists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName), cancellationToken);
        if (!exists)
        {
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName), cancellationToken);
        }

        var response = await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length),
            cancellationToken
        );
    }

    public async Task<Stream> GetFileAsync(string objectName, CancellationToken cancellationToken)
    {
        try
        {
            var ms = new MemoryStream();
            await minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(s => s.CopyTo(ms)),
                cancellationToken
            );
            ms.Position = 0;
            return ms;
        }
        catch (ObjectNotFoundException e)
        {
            throw new EntityNotFoundException("Файл не найден");
        }
    }
}