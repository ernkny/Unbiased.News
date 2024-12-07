using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.Playwright.Domain.Entities
{
    public class GeneratedNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Detail { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(50)]
        public string CreatedUser { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsApproved { get; set; }

        [MaxLength(255)]
        public string ApproveUser { get; set; }

        public DateTime? ApproveDate { get; set; }

        public string MatchId { get; set; }

    }
}
