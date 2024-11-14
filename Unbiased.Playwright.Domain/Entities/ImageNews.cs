using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.Playwright.Domain.Entities
{
    [Table("UB_NewsImage")]
    public class ImageNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string MatchId { get; set; }

        [Required]
        public string ImageBase64 { get; set; }
    }
}
