using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Represents a news entity.
/// </summary>
[Table("News")]
public class News
{
    /// <summary>
    /// Gets or sets the unique identifier for the news.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the news.
    /// </summary>
    /// <remarks>Required and limited to 255 characters.</remarks>
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the detailed content of the news.
    /// </summary>
    /// <remarks>Required and can be any length.</remarks>
    [Required]
    [Column(TypeName = "nvarchar(MAX)")]
    public string Detail { get; set; }

    /// <summary>
    /// Gets or sets the category identifier for the news.
    /// </summary>
    /// <remarks>Required.</remarks>
    [Required]
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the news was created.
    /// </summary>
    /// <remarks>Required.</remarks>
    [Required]
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the news was last modified.
    /// </summary>
    /// <remarks>Optional.</remarks>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets the user who created the news.
    /// </summary>
    /// <remarks>Optional and limited to 50 characters.</remarks>
    [MaxLength(50)]
    public string CreatedUser { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the news is active.
    /// </summary>
    /// <remarks>Required.</remarks>
    [Required]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the news is deleted.
    /// </summary>
    /// <remarks>Required.</remarks>
    [Required]
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the URL associated with the news.
    /// </summary>
    /// <remarks>Optional and can be any length.</remarks>
    [Column(TypeName = "nvarchar(MAX)")]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the match identifier for the news.
    /// </summary>
    /// <remarks>Optional.</remarks>
    public string? MatchId { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the news has been processed.
    /// </summary>
    /// <remarks>Required.</remarks>
    [Required]
    public bool IsProcessed { get; set; }
}

