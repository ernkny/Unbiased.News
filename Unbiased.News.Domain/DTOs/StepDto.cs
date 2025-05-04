namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing a step in a sequential process or guide.
    /// Used for step-by-step instructions within content.
    /// </summary>
    public class StepDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the step.
        /// </summary>
        public int StepId { get; set; }

        /// <summary>
        /// Gets or sets the position number of this step in the sequence.
        /// </summary>
        public int StepNumber { get; set; }

        /// <summary>
        /// Gets or sets the title or heading of the step.
        /// </summary>
        public string StepTitle { get; set; }

        /// <summary>
        /// Gets or sets the detailed description or instructions for the step.
        /// </summary>
        public string StepDescription { get; set; }
    }
}

