using Dapper;
using System.Data;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository for news-related operations.
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public NewsRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieves all generated news asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="GeneratedNew"/> objects.</returns>
        public async Task<IEnumerable<GeneratedNew>> GetAllGeneratedNewsAsync(string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Language", language);
                    return await connection.QueryAsync<GeneratedNew>("UB_sp_GetAllGeneratedNews", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="GenerateNewsWithImageDto"/> objects.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(int categoryId, int pageNumber,string language)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@CategoryId", categoryId);
                parameters.Add("@Language", language);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllGeneratedNewsWithImagePath", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllGeneratedNewsWithImageCountAsync(int categoryId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryId", categoryId);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_GetAllGeneratedNewsWithImagePathCount", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetBannerGeneratedNewsWithImageAsync(int categoryId,string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CategoryId", categoryId);
                    parameters.Add("@Language", language);
                    return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsForBannerWithImagePath",parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByIdWithImageAsync(string id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsById", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GenerateNewsWithImageDto> GetGeneratedNewsByUniqUrlWithImageAsync(string uniqUrlPath)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@uniqurlpath", uniqUrlPath);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<GenerateNewsWithImageDto>("UB_sp_GetGeneratedNewsByUniqUrlPath", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(int categoryId,string uniqUrlPath,string language)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", uniqUrlPath);
                parameters.Add("@categoryid", categoryId);
                parameters.Add("@language", language);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllLastTopGeneratedNewsWithCategoryId", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
