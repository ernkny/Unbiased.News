using Amazon.S3;
using Amazon.S3.Model;
using Unbiased.Dashboard.Domain.Model.Aws;

namespace Unbiased.Dashboard.Common.Concrete.Helpers
{
    public class SaveNewsImageToAws : IDisposable
    {
        private readonly AmazonS3Client client;
        private readonly HttpClient _httpClient;
        private bool disposedValue;

        public SaveNewsImageToAws(AwsCredentials awsCredentials)
        {
            _httpClient = new HttpClient(); ;
            client = new AmazonS3Client(awsCredentials.AccessKey, awsCredentials.SecretKey, Amazon.RegionEndpoint.EUNorth1);
        }

        public async Task<string> UploadFileToAws(string filePath, string bucketName, byte[] fileBytes)
        {
            try
            {
                var newGuid = Guid.NewGuid();
                var jpegFilePath = Path.Combine(filePath, $"{newGuid}.jpeg");
                using (var stream = new MemoryStream(fileBytes))
                {

                    await UploadToS3(stream, $"Pictures/{newGuid}.jpeg", "image/jpeg", bucketName);
                }
                return jpegFilePath;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async Task UploadToS3(MemoryStream stream, string key, string contentType, string bucketName)
        {
            stream.Position = 0;
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
                ContentType = contentType,
            };

            await client.PutObjectAsync(putRequest);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client?.Dispose();
                    _httpClient?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~SaveNewsImageToAws()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
