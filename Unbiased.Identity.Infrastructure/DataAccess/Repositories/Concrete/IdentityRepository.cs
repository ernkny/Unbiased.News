using Dapper;
using System.Data;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        public IdentityRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<bool> InsertRoleWithPermissions(RoleWithPermissionDto roleWithPermissionDto)
        {
            using (var connection = _connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RoleName", roleWithPermissionDto.RoleName);
                parameters.Add("@IsActive", roleWithPermissionDto.IsActive);

                var rolePermissionsTable = new DataTable();
                rolePermissionsTable.Columns.Add("PermissionId", typeof(int));
                rolePermissionsTable.Columns.Add("IsActive", typeof(bool));
                rolePermissionsTable.Columns.Add("IsDeleted", typeof(bool));

                foreach (var perm in roleWithPermissionDto.RolePermissionType)
                {
                    rolePermissionsTable.Rows.Add(perm.PermissionId, perm.IsActive, false);
                }

                parameters.Add("@RolePermissions", rolePermissionsTable.AsTableValuedParameter("RolePermissionType"));

                var result = await connection.ExecuteAsync("UB_sp_InsertRoleWithPermissions", parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            }
        }
    }
}
