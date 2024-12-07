using Newtonsoft.Json;

namespace Unbiased.Playwright.Domain.DTOs
{
    public class DalleImageGenerateResponseDto
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("data")]
        public ImageData[] Data { get; set; }

        public class ImageData
        {
            [JsonProperty("revised_prompt")]
            public string RevisedPrompt { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }
    }
}
