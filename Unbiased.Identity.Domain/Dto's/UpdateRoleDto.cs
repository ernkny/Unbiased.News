using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Identity.Domain.Dto_s
{
    public class UpdateRoleDto
    {
        [Required(ErrorMessage = "Role Id is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(100, ErrorMessage = "Role name cannot be longer than 100 characters")]
        public string RoleName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "At least one permission must be selected")]
        public List<int> PermissionIds { get; set; }

        public UpdateRoleDto()
        {
            PermissionIds = new List<int>();
        }
    }
}
