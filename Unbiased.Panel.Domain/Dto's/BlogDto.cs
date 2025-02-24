namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class BlogDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string MatchId { get; set; }
        public string Path { get; set; }
        public string UniqUrlPath { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    }
}
