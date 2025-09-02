using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

public class S3Servico
{
    private readonly IAmazonS3 _s3Client;

    public string BucketName { get; }

    public S3Servico(IConfiguration config)
    {
        var accessKey = config["AWS:AccessKey"];
        var secretKey = config["AWS:SecretKey"];
        var region = RegionEndpoint.GetBySystemName(config["AWS:Region"]);
        BucketName = config["AWS:BucketName"]; // Propriedade pública

        _s3Client = new AmazonS3Client(accessKey, secretKey, region);
    }

    public async Task UploadFileAsync(string keyName, Stream fileStream, string contentType)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = keyName,
            BucketName = BucketName,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };

        await fileTransferUtility.UploadAsync(uploadRequest);
    }

    public string GerarPresignedUrl(string keyName, string contentType)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Key = keyName,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(10),
            ContentType = contentType
        };

        return _s3Client.GetPreSignedURL(request);
    }
}
