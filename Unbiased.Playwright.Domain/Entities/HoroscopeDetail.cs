namespace Unbiased.Playwright.Domain.Entities
{
    /// <summary>
    /// Represents a detailed horoscope information.
    /// </summary>
    public class HoroscopeDetail
    {
        /// <summary>
        /// Unique identifier for the horoscope detail.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key referencing the Horoscope entity.
        /// </summary>
        public int HoroscopeId { get; set; }

        /// <summary>
        /// Detailed text of the horoscope.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Date and time when the horoscope detail was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Flag indicating whether the horoscope detail is deleted.
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Flag indicating whether the horoscope detail is approved.
        /// </summary>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// Date and time when the horoscope detail was last modified.
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
