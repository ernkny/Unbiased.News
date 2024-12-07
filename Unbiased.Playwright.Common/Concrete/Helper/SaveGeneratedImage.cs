using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Common.Concrete.Helper
{
    public class SaveGeneratedImage : IDisposable
    {
        private readonly HttpClient _httpClient;

        public SaveGeneratedImage()
        {
            _httpClient = new HttpClient(new HttpClientHandler());
        }

        public async Task<byte[]> SaveGeneratedImageAsBase64(string url, CancellationToken cancellationToken)
        {
            using (var response = await _httpClient.GetAsync(url, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
