namespace Unbiased.News.Domain.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
