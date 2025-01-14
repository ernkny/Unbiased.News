using Newtonsoft.Json;

namespace Unbiased.Playwright.Domain.DTOs
{
    public class HoroscopeResponseDto
    {
        [JsonProperty("detail")]
        public string Detail { get; set; }
    }
}
