namespace Unbiased.Playwright.Domain.Entities
{
    public class Contents
    {
        public int Id { get; set; } 
        public int ContentCategoryId { get; set; } 
        public string ContentDetail { get; set; } 
        public bool? IsActive { get; set; } 
        public bool? IsDeleted { get; set; } 
        public DateTime? CreatedDate { get; set; } 
        public DateTime? ModifiedDate { get; set; }
    }
}
