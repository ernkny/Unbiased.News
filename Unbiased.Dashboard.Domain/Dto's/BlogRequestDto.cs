namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class BlogRequestDto
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool? IsApproved { get; set; }
        public int? CategoryId { get; set; }
        public string SearchData { get; set; }
        public string Language { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
