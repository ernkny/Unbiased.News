using Amazon.S3;
using Amazon.S3.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
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
                using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException($"Failed to download image from {url}, Status code: {response.StatusCode}");

                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    var newGuid = Guid.NewGuid();
                    var jpgFilePath = Path.Combine(picturesPath, $"{newGuid}.jpg");
                    var jpegFilePath = Path.Combine(picturesPath, $"{newGuid}.jpeg");

                    // Upload original image as .jpg
                    using (var originalStream = new MemoryStream(imageBytes))
                    {
                        await UploadToS3(originalStream, $"Pictures/{newGuid}.jpg", "image/jpeg", bucketName);
                    }

                    // Convert and upload as .jpeg
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
