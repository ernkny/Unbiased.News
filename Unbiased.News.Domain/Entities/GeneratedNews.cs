using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.News.Domain.Entities
{
    /// <summary>
    /// Represents a generated news entity.
    /// </summary>
    public class GeneratedNews
    {
        /// <summary>
        /// Unique identifier for the news.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the news.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the news.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Category identifier for the news.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Date and time when the news was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Date and time when the news was last modified (nullable).
        /// </summary>
        public DateTime? ModifiedDate { get; set; } // Nullable because it's not always set

        /// <summary>
        /// User who created the news.
        /// </summary>
        public string CreatedUser { get; set; } // Not nullable, always set

        /// <summary>
        /// Flag indicating whether the news is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Flag indicating whether the news is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Flag indicating whether the news is approved.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// User who approved the news (nullable).
        /// </summary>
        public string ApproveUser { get; set; } // Nullable because it's not always set

        /// <summary>
        /// Date and time when the news was approved (nullable).
        /// </summary>
        public DateTime? ApproveDate { get; set; } // Nullable because it's not always set

        /// <summary>
        /// Match identifier for the news (nullable).
        /// </summary>
        public string MatchId { get; set; } // Nullable because it's not always set
    }
}
