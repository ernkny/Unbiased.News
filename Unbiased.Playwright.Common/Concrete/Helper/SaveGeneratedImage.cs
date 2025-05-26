using Microsoft.Extensions.Configuration;
using System;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Common.Concrete.Helper
{
    /// <summary>
    /// A class responsible for saving generated images to a file.
    /// </summary>
    public class SaveGeneratedImage : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveGeneratedImage"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public SaveGeneratedImage(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _httpClient = new HttpClient(new HttpClientHandler());
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SaveGeneratedImage"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        public async Task<string> SaveGeneratedImageToFile(string url, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                string filePath = Path.Combine($"{_configuration.GetSection("Paths:ImageFilePath").Value}", $"{Guid.NewGuid()}.jpg");
                using (var response = await _httpClient.GetAsync(url, cancellationToken))
                {
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(url);

                    await File.WriteAllBytesAsync(filePath, imageBytes, cancellationToken);
                }
                return filePath;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }

        }

        /// <summary>
        /// Releases the resources used by the <see cref="SaveGeneratedImage"/> class.
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
