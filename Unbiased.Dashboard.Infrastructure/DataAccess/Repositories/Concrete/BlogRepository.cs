using Dapper;
using System.Data;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Represents a repository for managing blog entries in the Unbiased Dashboard.
    /// </summary>
    public class BlogRepository : IBlogRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public BlogRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///  Retrieves all blog entries with pagination and filtering options for the dashboard.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BlogDto>> GetAllBlogsAsync(BlogRequestDto blogRequestDto, int pageNumber, int pageSize)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@PageNumber", pageNumber);
                paramaters.Add("@PageSize", pageSize);
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);
                paramaters.Add("@SearchData", blogRequestDto.SearchData);
                paramaters.Add("@StartDate", blogRequestDto.StartDate);
                paramaters.Add("@Language", blogRequestDto.Language.ToUpper());
                paramaters.Add("@EndDate", blogRequestDto.EndDate);

                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<BlogDto>("UB_sp_GetAllBlogsWithImageForDashboard", paramaters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the total count of blog entries based on the provided filter criteria.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <returns></returns>
        public async Task<int> GetAllBlogsCountAsync(BlogRequestDto blogRequestDto)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);
                paramaters.Add("@SearchData", blogRequestDto.SearchData);
                paramaters.Add("@StartDate", blogRequestDto.StartDate);
                paramaters.Add("@Language", blogRequestDto.Language.ToUpper());
                paramaters.Add("@EndDate", blogRequestDto.EndDate);

                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryFirstAsync<int>("UB_sp_GetAllBlogsWithImageForDashboardCount", paramaters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }

        }

        /// <summary>
        ///  Retrieves a blog entry by its ID, including its associated image.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogDto> GetBlogByIdWithImageAsync(string id)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", id);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<BlogDto>("UB_sp_GetBlogById", paramaters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        ///  Deletes a blog entry by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBlogByIdAsync(string id)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", id);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_DeleteBlog", paramaters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }

        /// <summary>
        /// Inserts a new blog entry into the database.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<bool> InsertBlogAsync(InsertBlogDtoRequest blogRequestDto, int UserId)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Title", blogRequestDto.Title);
                paramaters.Add("@Detail", blogRequestDto.Detail);
                paramaters.Add("@UserId", UserId);
                paramaters.Add("@Path", blogRequestDto.Path);
                paramaters.Add("@Language", blogRequestDto.Language.ToUpper());
                paramaters.Add("@CreatedTime", DateTime.UtcNow);
                paramaters.Add("@IsActive", blogRequestDto.IsActive);
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_InsertBlog", paramaters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }

        }

        /// <summary>
        /// Updates an existing blog entry in the database.
        /// </summary>
        /// <param name="blogRequestDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBlogAsync(UpdateBlogDtoRequest blogRequestDto)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", blogRequestDto.Id);
                paramaters.Add("@Title", blogRequestDto.Title);
                paramaters.Add("@Detail", blogRequestDto.Detail);
                paramaters.Add("@Path", blogRequestDto.Path);
                paramaters.Add("@Language", blogRequestDto.Language.ToUpper());
                paramaters.Add("@CreatedTime", DateTime.UtcNow);
                paramaters.Add("@IsActive", blogRequestDto.IsActive);
                paramaters.Add("@IsApproved", blogRequestDto.IsApproved);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_UpdateGeneratedBlog", paramaters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider);
                throw;
            }
        }
    }
}
