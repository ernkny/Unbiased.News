using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.ApiGateway.Domain.Entities
{
    public class Configurations
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Value { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted
        {
            get; set;

        }
    }
}
