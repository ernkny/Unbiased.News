using Dapper;
using System.Data;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    public class ContentRepository : IContentRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public ContentRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail)
        {
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);
            using (var connection = _connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Detail", horoscopeDetail.Detail, DbType.String, ParameterDirection.Input);
                parameters.Add("@HoroscopeId", horoscopeDetail.HoroscopeId, DbType.String, ParameterDirection.Input);
                parameters.Add("@CreatedDate", turkeyTime, DbType.DateTime);
                var result = await connection.ExecuteAsync("UB_sp_InsertHoroscopeDetail", parameters, commandType: CommandType.StoredProcedure);
                return result == 1;
            }
        }

        public async Task<bool> AddDailyContentInformationAsync(Contents content)
        {
            using (var connection = _connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ContentDetail", content.ContentDetail, DbType.String, ParameterDirection.Input);
                parameters.Add("@ContentCategoryId", content.ContentCategoryId, DbType.String, ParameterDirection.Input);
                parameters.Add("@CreatedDate", content.CreatedDate, DbType.DateTime);
                var result = await connection.ExecuteAsync("UB_sp_InsertContentDetail", parameters, commandType: CommandType.StoredProcedure);
                return result == 1;
            }
        }
    }
}
