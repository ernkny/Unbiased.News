namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Entity class representing a step in a multi-step process or guide.
    /// Used for tutorials, how-to guides, and procedural content.
    /// </summary>
    public class ContentStep
    {
        /// <summary>
        /// Gets or sets the number or position of this step in the sequence.
        /// </summary>
        public string StepNumber { get; set; }
        
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
