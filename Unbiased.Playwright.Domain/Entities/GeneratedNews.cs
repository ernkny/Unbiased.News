using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Represents a generated news entity.
    /// </summary>
    public class GeneratedNews
    {
        /// <summary>
        /// Unique identifier for the generated news.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the generated news.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Detailed content of the generated news.
        /// </summary>
        [Required]
        public string Detail { get; set; }

        /// <summary>
        /// Foreign key referencing the Category entity.
        /// </summary>
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Timestamp when the generated news was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Timestamp when the generated news was last modified (optional).
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Username of the user who created the generated news.
        /// </summary>
        [MaxLength(50)]
        public string CreatedUser { get; set; }

        /// <summary>
        /// Flag indicating whether the generated news is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Flag indicating whether the generated news is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Flag indicating whether the generated news is approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Username of the user who approved the generated news.
        /// </summary>
        [MaxLength(255)]
        public string ApproveUser { get; set; }

        /// <summary>
        /// Timestamp when the generated news was approved (optional).
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Match ID associated with the generated news.
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// Language of the generated news.
        /// </summary>
        public string Language { get; set; }
    }
}
