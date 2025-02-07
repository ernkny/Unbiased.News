using Unbiased.Identity.Domain.Entities;

namespace Unbiased.Identity.Domain.Dto_s
{
    public class GetUserWithRolesDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public bool IsActive { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }

}
