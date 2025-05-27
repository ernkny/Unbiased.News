using Dapper;
using System.Data;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository implementation for identity operations providing functionality for role and permission management with database access, error handling and logging.
    /// </summary>
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the IdentityRepository class.
        /// </summary>
        /// <param name="connection">The SQL connection provider for database operations.</param>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public IdentityRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Inserts a new role with its associated permissions into the system using stored procedure with error handling and logging.
        /// </summary>
        /// <param name="roleWithPermissionDto">The role with permission data transfer object containing role and permission information.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during role and permission insertion.</exception>
        public async Task<bool> InsertRoleWithPermissions(RoleWithPermissionDto roleWithPermissionDto)
        {
            try
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
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }

        }
    }
}
