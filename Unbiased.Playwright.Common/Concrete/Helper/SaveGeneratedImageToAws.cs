using Amazon.S3;
using Amazon.S3.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Common.Concrete.Helper
{
    /// <summary>
    /// A class responsible for downloading, processing, and uploading generated images to AWS S3.
    /// Implements IDisposable to properly manage resources.
    /// </summary>
    public class SaveGeneratedImageToAws : IDisposable
    {
        private readonly AmazonS3Client client;
        private readonly HttpClient _httpClient;
        private bool disposedValue;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveGeneratedImageToAws"/> class.
        /// </summary>
        /// <param name="awsCredentials">The AWS credentials for S3 access.</param>
        public SaveGeneratedImageToAws(AwsCredentials awsCredentials, IEventAndActivityLog eventAndActivityLog)
        {
            client = new AmazonS3Client(awsCredentials.AccessKey, awsCredentials.SecretKey, Amazon.RegionEndpoint.EUNorth1);
            _httpClient = new HttpClient();
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Downloads an image from a specified URL, processes it, and uploads both original and resized versions to AWS S3.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket to upload to.</param>
        /// <param name="picturesPath">The local pictures path for temporary storage.</param>
        /// <param name="url">The URL of the image to download.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The file path of the uploaded JPEG image.</returns>
        public async Task<string> GetFileFromGptAndUploadFileAsync(string bucketName, string picturesPath, string url, CancellationToken cancellationToken)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"Failed to download image from {url}, Status code: {response.StatusCode}");

                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    var newGuid = Guid.NewGuid();
                    var jpgFilePath = Path.Combine(picturesPath, $"{newGuid}.jpg");
                    var jpegFilePath = Path.Combine(picturesPath, $"{newGuid}.jpeg");
                    using (var originalStream = new MemoryStream(imageBytes))
                    {
                        await UploadToS3(originalStream, $"Pictures/{newGuid}.jpg", "image/jpeg", bucketName);
                    }

                    using (var streamForConversion = new MemoryStream(imageBytes))
                    using (var image = Image.Load(streamForConversion))
                    {
                        image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
                        using (var outputStream = new MemoryStream())
                        {
                            var encoder = new JpegEncoder { Quality = 75 };
                            image.Save(outputStream, encoder);
                            outputStream.Seek(0, SeekOrigin.Begin);
                            await UploadToS3(outputStream, $"Pictures/{newGuid}.jpeg", "image/jpeg", bucketName);
                        }
                    }

                    return jpegFilePath;
                }
            }
            catch (AmazonS3Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Uploads a memory stream to a specified S3 bucket.
        /// </summary>
        /// <param name="stream">The memory stream containing the file data.</param>
        /// <param name="key">The S3 object key (file path in S3).</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <param name="bucketName">The name of the S3 bucket to upload to.</param>
        /// <returns>A task that represents the asynchronous upload operation.</returns>
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

        /// <summary>
        /// Converts an image to JPEG format with compression and uploads it to S3.
        /// </summary>
        /// <param name="image">The image to convert and upload.</param>
        /// <param name="key">The S3 object key (file path in S3).</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <param name="bucketName">The name of the S3 bucket to upload to.</param>
        /// <returns>A task that represents the asynchronous convert and upload operation.</returns>
        private async Task ConvertAndUploadToS3(Image image, string key, string contentType, string bucketName)
        {
            using (var outputStream = new MemoryStream())
            {
                var encoder = new JpegEncoder { Quality = 75 };
                image.Save(outputStream, encoder);
                outputStream.Seek(0, SeekOrigin.Begin);

                await UploadToS3(outputStream, key, contentType, bucketName);
            }
        }

        /// <summary>
        /// Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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

        /// <summary>
        /// Finalizer that ensures resources are released when the object is garbage collected.
        /// </summary>
        ~SaveGeneratedImageToAws()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
