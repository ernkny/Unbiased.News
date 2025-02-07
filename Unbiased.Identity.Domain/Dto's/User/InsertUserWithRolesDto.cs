using Unbiased.Identity.Domain.Dto_s.User;

namespace Unbiased.Identity.Domain.Dto_s
{
    public class InsertUserWithRolesDto : UserBase
    {
        public string Password { get; set; }

        public override List<int> Roles { get; set; }

        public InsertUserWithRolesDto()
        {
            Roles = new List<int>();
        }
    }
}
