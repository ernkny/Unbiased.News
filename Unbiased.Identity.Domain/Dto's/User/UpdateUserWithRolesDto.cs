using Unbiased.Identity.Domain.Dto_s.User;

namespace Unbiased.Identity.Domain.Dto_s
{
    public class UpdateUserWithRolesDto : UserBase
    {
        public int UserId { get; set; }

        public override List<int> Roles { get; set; }

        public UpdateUserWithRolesDto()
        {
            Roles = new List<int>();
        }
    }
}
