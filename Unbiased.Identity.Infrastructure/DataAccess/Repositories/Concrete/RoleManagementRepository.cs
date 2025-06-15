using Dapper;
using System.Data;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Domain.Entities;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
	/// <summary>
	/// Repository implementation for role management operations providing comprehensive CRUD functionality for role and permission management with database access, error handling and logging.
	/// </summary>
	public class RoleManagementRepository : IRoleManagementRepository
	{
		private readonly UnbiasedSqlConnection _connection;
		private readonly IServiceProvider _serviceProvider;
		private readonly IEventAndActivityLog _eventAndActivityLog;

		/// <summary>
		/// Initializes a new instance of the RoleManagementRepository class.
		/// </summary>
		/// <param name="connection">The SQL connection provider for database operations.</param>
		/// <param name="serviceProvider">The service provider for dependency injection.</param>
		public RoleManagementRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
		{
			_connection = connection;
			_serviceProvider = serviceProvider;
			_eventAndActivityLog = eventAndActivityLog;
		}

		/// <summary>
		/// Retrieves all pages with their associated permissions from the system using stored procedure with error handling and logging.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation containing a collection of pages with permissions DTOs.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during pages with permissions retrieval.</exception>
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
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}

		}

		/// <summary>
		/// Retrieves all roles with pagination support using stored procedure with error handling and logging.
		/// </summary>
		/// <param name="pageNumber">The page number for pagination.</param>
		/// <param name="pageSize">The number of items per page for pagination.</param>
		/// <returns>A task that represents the asynchronous operation containing a collection of role entities.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during roles retrieval.</exception>
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
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves all roles without pagination using stored procedure with error handling and logging.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation containing a collection of all role entities.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during roles retrieval.</exception>
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
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Gets the total count of all roles in the system using stored procedure with error handling and logging.
		/// </summary>
		/// <returns>A task that represents the asynchronous operation containing the total count of roles.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during roles count retrieval.</exception>
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
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Deletes a role by its unique identifier using stored procedure with error handling and logging.
		/// </summary>
		/// <param name="id">The unique identifier of the role to delete.</param>
		/// <returns>A task that represents the asynchronous operation containing the number of affected rows.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during role deletion.</exception>
		public async Task<int> DeleteRoleAsync(int id)
		{
			try
			{
				using (var connection = _connection.CreateConnection())
				{
					var parameters = new DynamicParameters();
					parameters.Add("@RoleId", id);
					var result = await connection.ExecuteAsync("UBFMW_sp_DeleteRole", parameters, commandType: CommandType.StoredProcedure);
					return result;
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
				});
				throw;
			}
		}

		/// <summary>
		/// Creates a new role with assigned permissions in the system using stored procedure with error handling and logging.
		/// </summary>
		/// <param name="role">The create role data transfer object containing role and permission information.</param>
		/// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during role creation.</exception>
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
					parameters.Add("@PermissionIdListType", permissions.AsTableValuedParameter("PermissionIdListType"));

					var affectedRows = await connection.ExecuteAsync("UBFMW_sp_InsertRole", parameters, commandType: CommandType.StoredProcedure);
					return affectedRows > 0;
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
				});
				throw;
			}
		}

		/// <summary>
		/// Updates an existing role with new information and permission assignments using stored procedure with error handling and logging.
		/// </summary>
		/// <param name="role">The update role data transfer object containing updated role and permission information.</param>
		/// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during role update.</exception>
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
			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}
		}

		/// <summary>
		/// Retrieves a specific role with its assigned permissions by role identifier using stored procedure with error handling and logging.
		/// </summary>
		/// <param name="roleId">The unique identifier of the role to retrieve.</param>
		/// <returns>A task that represents the asynchronous operation containing the role DTO with detailed information and permissions.</returns>
		/// <exception cref="Exception">Thrown when an error occurs during role retrieval.</exception>
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

			catch (Exception exception)
			{
				await _eventAndActivityLog.SendEventLogToQueue(new EventLog
				{
					EventType = this.GetType().FullName,
					EventSeverity = "Error",
					Message = $"{exception.Message}",
					EventDate = DateTime.UtcNow
				});
				throw;
			}

		}
	}
}
