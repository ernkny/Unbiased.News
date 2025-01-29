using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Identity.Domain.Dto_s;
using Unbiased.Identity.Infrastructure.DataAccess.Connections;
using Unbiased.Identity.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Identity.Infrastructure.DataAccess.Repositories.Concrete
{
    public class RoleManagementRepository:IRoleManagementRepository
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
            using (var connection = _connection.CreateConnection())
            {
                var result = await connection.QueryAsync<PagesWithPermissionsDto>("UBFMW_sp_GetAllPagesWithPermissions", commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
