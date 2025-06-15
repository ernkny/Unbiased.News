using Amazon.S3;
using Amazon.S3.Model;
using Unbiased.Dashboard.Domain.Model.Aws;

namespace Unbiased.Dashboard.Common.Concrete.Helpers
{
    /// <summary>
    /// Provides functionality to save news images to Amazon Web Services (AWS) S3 storage.
    /// Implements IDisposable to properly manage AWS client resources.
    /// </summary>
    public class SaveNewsImageToAws : IDisposable
    {
        private readonly AmazonS3Client client;
        private readonly HttpClient _httpClient;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the SaveNewsImageToAws class with the specified AWS credentials.
        /// Creates an Amazon S3 client configured for the EU North 1 region.
        /// </summary>
        /// <param name="awsCredentials">The AWS credentials containing access key and secret key for S3 operations.</param>
        public SaveNewsImageToAws(AwsCredentials awsCredentials)
        {
            _httpClient = new HttpClient(); ;
            client = new AmazonS3Client(awsCredentials.AccessKey, awsCredentials.SecretKey, Amazon.RegionEndpoint.EUNorth1);
        }

        /// <summary>
        /// Uploads a file to AWS S3 storage asynchronously.
        /// Generates a unique GUID for the filename and stores the image in the Pictures folder.
        /// </summary>
        /// <param name="filePath">The local file path where the image should be stored.</param>
        /// <param name="bucketName">The name of the S3 bucket to upload the file to.</param>
        /// <param name="fileBytes">The byte array containing the image data to upload.</param>
        /// <returns>A task that represents the asynchronous upload operation. The task result contains the local file path with the generated GUID filename.</returns>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Disposes of the AWS S3 client and HTTP client resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
