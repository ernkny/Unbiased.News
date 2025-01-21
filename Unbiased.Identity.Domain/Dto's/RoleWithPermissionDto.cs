namespace Unbiased.Identity.Domain.Dto_s
{
    public class RoleWithPermissionDto
    {
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PermissionDto> RolePermissionType { get; set; } = new List<PermissionDto>();
    }
}
