namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing the response for content category titles.
    /// Used to hold the list of generated titles from AI services for content categories.
    /// </summary>
    public class ContentCategoryTitleResponse
    {
        /// <summary>
        /// Gets or sets the list of generated titles for content categories.
        /// These titles can be used as subheadings for content generation.
        /// </summary>
        public List<string> titles { get; set; }
    }
}
