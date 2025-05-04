namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Entity class representing a question and its corresponding answer within content.
    /// Used for FAQs, Q&A sections, and other question-based content structures.
    /// </summary>
    public class ContentQuestionAndAnswer
    {
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
