using Dapper;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    public class NewsImageRepository : INewsImageRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        public NewsImageRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<Guid> AddNewsImageAsync(InsertNewsImageDto addNewsImageDto)
        {
            using (var connection = _connection.CreateConnection())
            {
                var guid = Guid.NewGuid();
                var parameters = new DynamicParameters();
                parameters.Add("id", Guid.NewGuid(), DbType.Guid);
                parameters.Add("matchId", addNewsImageDto.MatchId, DbType.String);
                parameters.Add("imageBase64", addNewsImageDto.ImageBase64, DbType.String);

                await connection.ExecuteAsync("UB_sp_InsertNewsImage", parameters, commandType: CommandType.StoredProcedure);

                return guid;
            }
        }

        public async Task<IEnumerable<string>> GetNewsWithoutImages(DateTime startDate)
        {
            using (var connection = _connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("startDate", startDate, DbType.DateTime);

                return await connection.QueryAsync<string>("UB_sp_GetNewsWithoutImages", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
