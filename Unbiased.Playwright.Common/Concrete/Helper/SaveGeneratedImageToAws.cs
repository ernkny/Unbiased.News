using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Common.Concrete.Helper
{
    public class SaveGeneratedImageToAws : IDisposable
    {
        private readonly AmazonS3Client client;
        private readonly HttpClient _httpClient;
        private bool disposedValue;

        public SaveGeneratedImageToAws(AwsCredentials awsCredentials)
        {
            client = new AmazonS3Client(awsCredentials.AccessKey, awsCredentials.SecretKey, Amazon.RegionEndpoint.EUNorth1);
            _httpClient = new HttpClient();
        }

        public async Task<string> GetFileFromGptAndUploadFileAsync(string bucketName, string picturesPath, string url, CancellationToken cancellationToken)
        {
            try
            {
                byte[] imageBytes = null;
                using (var response = await _httpClient.GetAsync(url, cancellationToken))
                {
                     imageBytes = await _httpClient.GetByteArrayAsync(url);

                  
                }
                if (imageBytes == null) {
                    throw new ArgumentNullException("imageBytes is null");
                }
                var stream= new MemoryStream(imageBytes);
                var newguid = Guid.NewGuid();
                string filePath = Path.Combine(picturesPath, $"{newguid}.jpg");
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = $"Pictures/{newguid}.jpg",
                    InputStream = stream,
                    ContentType = "image/jpeg",
                };

                await client.PutObjectAsync(putRequest);
                return filePath;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' when writing an object to S3.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
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

        ~SaveGeneratedImageToAws()
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
