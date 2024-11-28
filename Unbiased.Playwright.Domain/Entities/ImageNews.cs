using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Represents an image news entity.
    /// </summary>
    [Table("UB_NewsImage")]
    public class ImageNews
    {
        /// <summary>
        /// Gets or sets the unique identifier for the image news.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the match identifier for the image news.
        /// </summary>
        /// <remarks>Required.</remarks>
        [Required]
        public string MatchId { get; set; }

        /// <summary>
        /// Gets or sets the base64 encoded image data.
        /// </summary>
        /// <remarks>Required.</remarks>
        [Required]
        public string ImageBase64 { get; set; }
    }
}
