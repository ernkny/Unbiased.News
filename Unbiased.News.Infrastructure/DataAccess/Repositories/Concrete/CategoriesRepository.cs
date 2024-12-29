using Dapper;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository class for managing categories.
    /// </summary>
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public CategoriesRepository(UnbiasedSqlConnection connection)
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
