using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Domain.Entities
{
    [Table("News")]
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Detail { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(50)]
        public string CreatedUser { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Url { get; set; }

        public string? MatchId { get; set; }

        [Required]
        public bool IsProcessed { get; set; }
    }
}
