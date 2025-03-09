namespace Unbiased.Dashboard.Domain.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
