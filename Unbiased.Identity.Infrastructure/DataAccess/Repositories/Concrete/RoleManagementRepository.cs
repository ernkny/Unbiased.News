using Dapper;
using System.Data;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
    public class RoleManagementRepository : IRoleManagementRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public RoleManagementRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PagesWithPermissionsDto>> GetAllPagesWithPermissionsAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<PagesWithPermissionsDto>("UBFMW_sp_GetAllPagesWithPermissions", commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync(int pageNumber, int pageSize)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PageNumber", pageNumber);
                    parameters.Add("@PageSize", pageSize);
                    var result = await connection.QueryAsync<Role>("UBFMW_sp_GetAllRoles", parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolessWithoutPaginationAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<Role>("UBFMW_sp_GetAllRolesWithoutPagination", commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllRolesCountAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryFirstAsync<int>("UBFMW_sp_GetAllRolesCount", commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteRoleAsync(int id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@RoleId", id);
                    var result = await connection.ExecuteAsync("UBFMW_sp_DeleteRole", parameters,commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> InsertRoleAsync(CreateRoleDto role)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var permissions = new DataTable();
                    permissions.Columns.Add("PermissionId", typeof(int));

                    foreach (var id in role.PermissionIds)
                    {
                        permissions.Rows.Add(id);
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@RoleName", role.RoleName);
                    parameters.Add("@Description", role.Description);
                    parameters.Add("@IsActive", role.IsActive);
                    parameters.Add("@@PermissionIdListType", permissions.AsTableValuedParameter("PermissionIdListType"));

                    var affectedRows = await connection.ExecuteAsync("UBFMW_sp_InsertRole", parameters, commandType: CommandType.StoredProcedure);
                    return affectedRows > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateRoleAsync(UpdateRoleDto role)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var permissions = new DataTable();
                    permissions.Columns.Add("PermissionId", typeof(int));

                    foreach (var id in role.PermissionIds)
                    {
                        permissions.Rows.Add(id);
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@RoleId", role.RoleId);
                    parameters.Add("@RoleName", role.RoleName);
                    parameters.Add("@Description", role.Description);
                    parameters.Add("@IsActive", role.IsActive);
                    parameters.Add("@@PermissionIdListType", permissions.AsTableValuedParameter("PermissionIdListType"));

                    var affectedRows = await connection.QueryFirstAsync<int>("UBFMW_sp_UpdateRole", parameters, commandType: CommandType.StoredProcedure);
                    return affectedRows > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RoleGetByIdDto> GetRoleById(int roleId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var roleDictionary = new Dictionary<int, RoleGetByIdDto>();

                    var list = await connection.QueryAsync<RoleGetByIdDto, int, RoleGetByIdDto>(
                        "UBFMW_sp_GetRolesWithPermissionsById",
                        (role, permissionId) =>
                        {
                            RoleGetByIdDto roleEntry;

                            if (!roleDictionary.TryGetValue(role.RoleId, out roleEntry))
                            {
                                roleEntry = role;
                                roleEntry.PermissionIds = new List<int>();
                                roleDictionary.Add(role.RoleId, roleEntry);
                            }

                            roleEntry.PermissionIds.Add(permissionId);
                            return roleEntry;
                        },
                        new { RoleId = roleId },
                        commandType: CommandType.StoredProcedure,
                        splitOn: "PermissionId"
                    );

                    return roleDictionary.Values.FirstOrDefault();
                }
            }

            catch (Exception)
            {

                throw;
            }

        }
    }
}
