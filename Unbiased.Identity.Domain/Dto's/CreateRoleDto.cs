using System.ComponentModel.DataAnnotations;

namespace Unbiased.Identity.Domain.Dto_s
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(100, ErrorMessage = "Role name cannot be longer than 100 characters")]
        public string RoleName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "At least one permission must be selected")]
        public List<int> PermissionIds { get; set; }

        public CreateRoleDto()
        {
            PermissionIds = new List<int>();
        }
    }
}
