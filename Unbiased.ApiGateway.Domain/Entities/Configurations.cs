using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unbiased.ApiGateway.Domain.Entities
{
    /// <summary>
    /// Represents a configuration entity.
    /// </summary>
    public class Configurations
    {
        /// <summary>
        /// Gets or sets the unique identifier for the configuration.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        [Column(TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the configuration.
        /// </summary>
        [Column(TypeName = "nvarchar(MAX)")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
       
    }
}
