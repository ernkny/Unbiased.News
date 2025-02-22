using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public CategoryRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }
        /// <summary>
        /// Retrieves all categories from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of categories.</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                var result = await connection.QueryAsync<Category>($"Exec UB_sp_GetAllcategoriesWithCount");
                return result;
            }
        }

    }
}
