namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class GenerateNewsWithImageDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// Detailed content of the generated news.
        /// </summary>
        public string Detail { get; set; }

        public int CategoryId { get; set; }

        /// <summary>
        /// Foreign key referencing the Category entity.
        /// </summary>
        public string CategoryName{ get; set; }

        /// <summary>
        /// Match ID associated with the generated news.
        /// </summary>
        public string MatchId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Langauge { get; set; }

        public string Path { get; set; }

        public string UniqUrlPath { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    }
}
