using Dapper;
using System.Data;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository for news image related operations.
    /// </summary>
    public class NewsImageRepository : INewsImageRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsImageRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public NewsImageRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Adds a new news image to the database.
        /// </summary>
        /// <param name="addNewsImageDto">The news image to add.</param>
        /// <returns>The ID of the newly added news image.</returns>
        public async Task<bool> AddNewsImageAsync(InsertNewsImageDto addNewsImageDto)
        {
            using (var connection = _connection.CreateConnection())
            {
                var guid = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("id", Guid.NewGuid(), DbType.Guid);
                parameters.Add("matchId", addNewsImageDto.MatchId, DbType.String);
                parameters.Add("path", addNewsImageDto.filePath, DbType.String);

               var result= await connection.ExecuteAsync("UB_sp_InsertNewsImage", parameters, commandType: CommandType.StoredProcedure);

                return result==1;
            }
        }

        /// <summary>
        /// Retrieves all news images that do not have an image.
        /// </summary>
        /// <param name="startDate">The start date for filtering.</param>
        /// <returns>A list of news images without images.</returns>
        public async Task<IEnumerable<GetNewsWithoutImageDto>> GetNewsWithoutImages(DateTime startDate)
        {
            using (var connection = _connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("startDate", startDate, DbType.DateTime);

                return await connection.QueryAsync<GetNewsWithoutImageDto>("UB_sp_GetNewsWithoutImages", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
