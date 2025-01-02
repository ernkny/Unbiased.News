using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    public class SearchUrlRepository: ISearchUrlRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchUrlRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public SearchUrlRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieves all active keywords for search.
        /// </summary>
        /// <returns>A list of active keywords for search.</returns>
        public async Task<IEnumerable<ActiveUrlsForSearchDto>> GetAllActiveUrlsForSearchAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {
                    return await connection.QueryAsync<ActiveUrlsForSearchDto>("UB_sp_GetAllActiveUrls", commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves all active keywords for search.
        /// </summary>
        /// <returns>A list of active keywords for search.</returns>
        public async Task<bool> UpdateAllSearhcUrlNextRunTimeAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {

                    return await connection.ExecuteAsync("UB_sp_UpdateSearchUrlNextRun", commandType: CommandType.StoredProcedure)==1;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
