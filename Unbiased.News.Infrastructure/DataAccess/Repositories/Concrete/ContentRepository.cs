using Dapper;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    public class ContentRepository : IContentRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public ContentRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Contents> GetLastContentAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                var result = await connection.QueryFirstAsync<Contents>($"Exec UB_sp_GetLastDailyContent");
                return result;
            }
        }
        public async Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                var result = await connection.QueryAsync<HoroscopeDailyDetail>($"Exec UB_sp_GetDailyLastHoroscopes");
                return result;
            }
        }

    }
}
