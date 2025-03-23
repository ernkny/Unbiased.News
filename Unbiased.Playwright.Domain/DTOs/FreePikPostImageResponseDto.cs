namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing the response from the Freepik API for image generation requests.
    /// This class encapsulates the response structure from the Freepik API.
    /// </summary>
    public class FreePikPostImageResponseDto
    {
        /// <summary>
        /// Contains the detailed data of the response from the Freepik API.
        /// </summary>
        public FreePikPostImageResponse Data { get; set; } = new FreePikPostImageResponse();

        /// <summary>
        /// Nested class representing the detailed data structure of the Freepik API response.
        /// </summary>
        public class FreePikPostImageResponse
        {
            /// <summary>
            /// The unique identifier for the image generation task.
            /// Used for retrieving the generated image in subsequent API calls.
            /// </summary>
            public string task_id { get; set; }
            
            /// <summary>
            /// The current status of the image generation task (e.g., "completed", "processing").
            /// </summary>
            public string status { get; set; }
            
            /// <summary>
            /// An array of URLs pointing to the generated images.
            /// </summary>
            public string[] generated { get; set; }
        }
    }
  
}
