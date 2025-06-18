using Dapper;
using System.Data;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    ///  Repository class for managing blog-related data access operations.
    /// </summary>
    public class BlogRepository : IBlogRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public BlogRepository(UnbiasedSqlConnection connection, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all generated news with images asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="BlogWithImageDto"/> objects.</returns>
        public async Task<IEnumerable<BlogWithImageDto>> GetAllBlogsWithImageAsync(string language, int pageNumber, string searchData)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@SearchData", searchData);
                parameters.Add("@Language", language);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<BlogWithImageDto>("UB_sp_GetAllBlogsWithImageForMainPage", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves the count of all blogs with images for the main page asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        public async Task<int> GetAllBlogsWithImageCountAsync(string language, string? searchData)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@SearchData", searchData);
                parameters.Add("@Language", language);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_GetAllBlogsWithImageForMainPageCount", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
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
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }
    }
}
