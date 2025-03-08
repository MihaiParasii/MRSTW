using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using MRSTW.Api.Contracts;

namespace MRSTW.Api.Services;

public class AmazonS3Service(IAmazonS3 s3Client)
{
    private const string BucketName = "mrstw";

    public async Task<UploadFilesResponse> UploadFilesAsync(UploadFilesRequest request)
    {
        bool bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, BucketName);

        if (!bucketExists)
        {
            var bucketRequest = new PutBucketRequest()
            {
                BucketName = BucketName,
                UseClientRegion = true
            };
            await s3Client.PutBucketAsync(bucketRequest);
        }

        List<string> filePaths = [];

        foreach (var file in request.Files)
        {
            await using var fileStream = file.OpenReadStream();
            string filePath = CreateFilePath(request.UserId, file.FileName);
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = filePath,
                InputStream = fileStream,
                StorageClass = S3StorageClass.Standard
            };
            filePaths.Add(filePath);
            await s3Client.PutObjectAsync(putObjectRequest);
        }

        return new UploadFilesResponse(filePaths);
    }

    public async Task DeleteFilesAsync(List<string> filePaths)
    {
        foreach (string filePath in filePaths)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = filePath
            };
            
            await s3Client.DeleteObjectAsync(deleteObjectRequest);
        }
    }

    private static string CreateFilePath(Guid userId, string fileName)
    {
        return $"{userId}/{DateTime.Now:yyyy/MM/dd/hhmmss}–{fileName}";
    }
}
