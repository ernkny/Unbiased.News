namespace Unbiased.Identity.Domain.Dto_s
{
    public class PagesWithPermissionsDto
    {
        public string PageName { get; set; }
        public int PermissionId { get; set; }
        public int PageId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string PermissionType { get; set; }
    }
}
