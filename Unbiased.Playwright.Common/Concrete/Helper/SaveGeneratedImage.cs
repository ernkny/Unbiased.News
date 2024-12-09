using Microsoft.Extensions.Configuration;

namespace Unbiased.Playwright.Common.Concrete.Helper
{
    public class SaveGeneratedImage : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SaveGeneratedImage(IConfiguration configuration)
        {
            _httpClient = new HttpClient(new HttpClientHandler());
            _configuration = configuration;
        }

        public async Task<string> SaveGeneratedImageToFile(string url,string fileName, CancellationToken cancellationToken)
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
            catch (Exception)
            {

                throw;
            }
          
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
