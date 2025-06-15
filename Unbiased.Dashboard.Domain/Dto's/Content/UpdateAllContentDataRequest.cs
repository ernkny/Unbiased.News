namespace Unbiased.Dashboard.Domain.Dto_s.Content
{
    /// <summary>
    /// Represents a data transfer object for updating all content data in the system.
    /// Contains all necessary information to update content including metadata, categories, and associated questions and steps.
    /// </summary>
    public class UpdateAllContentDataRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the content sub-heading.
        /// </summary>
        public int ContentSubHeadingId { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the content category.
        /// </summary>
        public int ContentCategoryId { get; set; }
        
        /// <summary>
        /// Gets or sets the title of the content.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the content is active.
        /// Default value is true.
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Gets or sets the unique URL path for the content.
        /// </summary>
        public string UniqUrlPath { get; set; }
        
        /// <summary>
        /// Gets or sets the creation time of the content.
        /// </summary>
        public DateTime? CreatedTime { get; set; }
        
        /// <summary>
        /// Gets or sets the subtitle of the content.
        /// </summary>
        public string SubTitle { get; set; }
        
        /// <summary>
        /// Gets or sets the prompt used for generating the content image.
        /// </summary>
        public string ImagePrompt { get; set; }
        
        /// <summary>
        /// Gets or sets the hashtags associated with the content.
        /// </summary>
        public string Hashtags { get; set; }
        
        /// <summary>
        /// Gets or sets the file path or URL of the content image.
        /// </summary>
        public string ImagePath { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the content details are active.
        /// Default value is true.
        /// </summary>
        public bool? DetailIsActive { get; set; } = true;
        
        /// <summary>
        /// Gets or sets a value indicating whether the content details are deleted.
        /// Default value is false.
        /// </summary>
        public bool? DetailIsDeleted { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the list of questions associated with the content.
        /// Initialized as an empty list.
        /// </summary>
        public List<QuestionDto> Questions { get; set; } = new();
        
        /// <summary>
        /// Gets or sets the list of steps associated with the content.
        /// Initialized as an empty list.
        /// </summary>
        public List<StepDto> Steps { get; set; } = new();
    }
}
