using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing a comprehensive content data request.
    /// Contains all necessary information to insert generated content into the database,
    /// including metadata, content details, and associated elements like questions and steps.
    /// </summary>
    public class InsertAllContentDataRequest
    {
        /// <summary>
        /// Gets or sets the ID of the content category this content belongs to.
        /// </summary>
        public int ContentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the content subheading this content is associated with.
        /// </summary>
        public int ContentSubHeadingId { get; set; }

        /// <summary>
        /// Gets or sets the title of the content.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is active.
        /// Default is true.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the content has been processed.
        /// Default is false.
        /// </summary>
        public bool IsProccessed { get; set; } = false;

        /// <summary>
        /// Gets or sets the subtitle or secondary heading of the content.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Gets or sets the prompt text used to generate the content's image.
        /// </summary>
        public string ImagePrompt { get; set; }

        /// <summary>
        /// Gets or sets the hashtags associated with the content.
        /// </summary>
        public string Hashtags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content detail is active.
        /// Default is true.
        /// </summary>
        public bool DetailIsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the content detail is deleted.
        /// Default is false.
        /// </summary>
        public bool DetailIsDeleted { get; set; } = false;

        /// <summary>
        /// Gets or sets the file path or URL to the content's image.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the list of questions and answers related to this content.
        /// Default is an empty list.
        /// </summary>
        public List<ContentQuestionAndAnswer> Questions { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of step-by-step guides related to this content.
        /// Default is an empty list.
        /// </summary>
        public List<ContentStep> Steps { get; set; } = new();
    }
}
