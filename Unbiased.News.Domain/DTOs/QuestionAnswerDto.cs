namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for question and answer pairs related to matches.
    /// </summary>
    public class QuestionAnswerDto
    {
        /// <summary>
        /// Unique identifier for the question-answer pair.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The question text.
        /// </summary>
        public string Question { get; set; }
        
        /// <summary>
        /// The answer text corresponding to the question.
        /// </summary>
        public string Answer { get; set; }
        
        /// <summary>
        /// The date and time when the question-answer pair was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Indicates whether the question-answer pair is active.
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Indicates whether the question-answer pair has been marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// The unique identifier of the match this question-answer pair is associated with.
        /// </summary>
        public string MatchId { get; set; }
    }
}
