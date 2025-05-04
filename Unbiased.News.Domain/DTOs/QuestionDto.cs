namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing a question and its corresponding answer.
    /// Used for content-related FAQs and similar Q&A structures.
    /// </summary>
    public class QuestionDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the question.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the text of the answer to the question.
        /// </summary>
        public string Answer { get; set; }
    }
}
