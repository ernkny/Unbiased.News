using Dapper;
using System.Data;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    public class BlogRepository:IBlogRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public BlogRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="BlogWithImageDto"/> objects.</returns>
        public async Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(int pageNumber,string searchData)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@SearchData", searchData);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<BlogWithImageDto>("UB_sp_GetAllBlogsWithImageForMainPage", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> GetAllBlogsWithImageCountAsync(string? searchData)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SearchData", searchData);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_GetAllBlogsWithImageForMainPageCount", parameters, commandType: CommandType.StoredProcedure);
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
        /// <returns>A collection of <see cref="BlogWithImageDto"/> objects.</returns>
        public async Task<BlogWithImageDto> GetBlogWithImageByUniqUrlAsync(string @UniqUrl)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UniqUrl", @UniqUrl);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<BlogWithImageDto>("UB_sp_GetBlogByUniqUrl", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
