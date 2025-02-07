namespace Unbiased.Identity.Domain.Dto_s.User
{
    public abstract class UserBase
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public bool IsActive { get; set; }
        public abstract List<int> Roles { get; set; }
    }
}
