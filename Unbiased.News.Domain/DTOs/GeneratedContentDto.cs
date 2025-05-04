namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object that represents generated content with its associated details.
    /// Contains the content's metadata, details, questions and answers, and step-by-step guides.
    /// </summary>
    public class GeneratedContentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the subheading.
        /// </summary>
        public int SubHeadingId { get; set; }

        /// <summary>
        /// Gets or sets the title of the subheading.
        /// </summary>
        public string SubHeadingTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the content category this content belongs to.
        /// </summary>
        public int ContentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content has been processed.
        /// </summary>
        public bool IsProccessed { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of the content.
        /// </summary>
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the unique URL path used to access this content.
        /// </summary>
        public string UniqUrlPath { get; set; }

        /// <summary>
        /// Gets or sets the detailed content information.
        /// </summary>
        public ContentDetailDto ContentDetail { get; set; }

        /// <summary>
        /// Gets or sets the list of questions and answers related to this content.
        /// </summary>
        public List<QuestionDto> QuestionsAndAnswers { get; set; }

        /// <summary>
        /// Gets or sets the list of step-by-step guides related to this content.
        /// </summary>
        public List<StepDto> Steps { get; set; }
    }
}
