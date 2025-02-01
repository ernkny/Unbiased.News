using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Identity.Domain.Dto_s
{
    public class RoleGetByIdDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public List<int> PermissionIds { get; set; }

        public RoleGetByIdDto()
        {
            PermissionIds = new List<int>();
        }
    }
}
