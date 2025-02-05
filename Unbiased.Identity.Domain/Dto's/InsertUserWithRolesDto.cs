namespace Unbiased.Identity.Domain.Dto_s
{
    public class InsertUserWithRolesDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public bool IsActive { get; set; }

        public List<int> Roles { get; set; }

        public InsertUserWithRolesDto()
        {
            Roles = new List<int>();
        }
    }
}
