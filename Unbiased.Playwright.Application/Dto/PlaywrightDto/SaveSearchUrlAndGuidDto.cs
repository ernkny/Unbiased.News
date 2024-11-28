namespace Unbiased.Playwright.Application.Dto.PlaywrightDto
{
    /// <summary>
    /// Represents a data transfer object for saving search URLs and GUIDs.
    /// </summary>
    public class SaveSearchUrlAndGuidDto
    {
        /// <summary>
        /// Gets or sets the title of the search result.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL of the search result.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the match ID of the search result.
        /// </summary>
        public string? MatchId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search result has been processed.
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the search result is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the search result was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
