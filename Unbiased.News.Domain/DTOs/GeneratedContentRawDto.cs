namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing raw generated content with string-based properties.
    /// Used to handle serialized JSON data from the database before deserializing into structured objects.
    /// </summary>
    public class GeneratedContentRawDto
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
        ///  Gets or sets the language in which the content is written.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the JSON string representation of the content details.
        /// This will be deserialized into a ContentDetailDto object.
        /// </summary>
        public string ContentDetail { get; set; }

        /// <summary>
        /// Gets or sets the JSON string representation of questions and answers.
        /// This will be deserialized into a List of QuestionDto objects.
        /// </summary>
        public string QuestionsAndAnswers { get; set; }

        /// <summary>
        /// Gets or sets the JSON string representation of step-by-step guides.
        /// This will be deserialized into a List of StepDto objects.
        /// </summary>
        public string Steps { get; set; }
    }
}
