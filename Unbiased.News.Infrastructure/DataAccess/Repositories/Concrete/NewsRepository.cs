using Dapper;
using System.Data;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    public class NewsRepository:INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        public NewsRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<GeneratedNews>> GetAllGeneratedNewsAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    return await connection.QueryAsync<GeneratedNews>("UB_sp_GetAllGeneratedNews", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllGeneratedNewsWithImagePath", commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
