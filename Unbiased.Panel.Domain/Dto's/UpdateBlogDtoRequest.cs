namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class UpdateBlogDtoRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string Path { get; set; }
    }
}
