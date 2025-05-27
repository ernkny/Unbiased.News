using Dapper;
using System;
using System.Data;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    ///  Represents a repository for managing news articles with images.
    /// </summary>
    public class NewsRepository:INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public NewsRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves all generated news articles with images for the dashboard.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> GetAllGeneratedNewsWithImageAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters(); 
                    parameters.Add("@pageNumber", requestDto.PageNumber);
                    parameters.Add("@pageSize", requestDto.PageSize);
                    parameters.Add("@Language", requestDto.Language);
                    parameters.Add("@IsApproved", requestDto.IsApproved);
                    parameters.Add("@categoryId", requestDto.CategoryId);
                    parameters.Add("@SearchData", requestDto.SearchData);
                    parameters.Add("@StartDate", requestDto.StartDate);
                    parameters.Add("@EndDate", requestDto.EndDate);
                    return await connection.QueryAsync<GenerateNewsWithImageDto>("UB_sp_GetAllGeneratedNewsWithImagePathForDashboard", parameters, commandType: CommandType.StoredProcedure);
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
        ///  Retrieves the count of all generated news articles with images for the dashboard.
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> GetAllGeneratedNewsWithImageCountAsync(GetGeneratedNewsWithImagePathRequestDto requestDto)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Language", requestDto.Language);
                    parameters.Add("@IsApproved", requestDto.IsApproved);
                    parameters.Add("@categoryId", requestDto.CategoryId);
                    parameters.Add("@SearchData", requestDto.SearchData);
                    parameters.Add("@StartDate", requestDto.StartDate);
                    parameters.Add("@EndDate", requestDto.EndDate);
                    return await connection.QueryFirstAsync<int>("UB_sp_GetAllGeneratedNewsWithImagePathForDashboardCount", parameters, commandType: CommandType.StoredProcedure);
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
        /// Retrieves a specific generated news article with an image by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Deletes a news article by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteNewsByIdAsync(string id)
        {
            try
            {
                var paramaters = new DynamicParameters();
                paramaters.Add("@Id", id);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_DeleteNews", paramaters, commandType: CommandType.StoredProcedure)==1;
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
        /// Updates a generated news article with an image.
        /// </summary>
        /// <param name="generatedNewsDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateGeneratedNewsWithImageAsync(UpdateGeneratedNewsDto generatedNewsDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", generatedNewsDto.Id,DbType.String);
                parameters.Add("@Title", generatedNewsDto.Title,DbType.String);
                parameters.Add("@Detail", generatedNewsDto.Detail,DbType.String);
                parameters.Add("@CategoryId", generatedNewsDto.CategoryId,DbType.Int32);
                parameters.Add("@CreatedTime", generatedNewsDto.CreatedTime,DbType.DateTime);
                parameters.Add("@IsApproved", generatedNewsDto.IsApproved,DbType.Boolean);
                parameters.Add("@IsActive", generatedNewsDto.IsActive,DbType.Boolean);
                parameters.Add("@ImagePath", generatedNewsDto.ImagePath,DbType.String);
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_UpdateGeneratedNews", parameters, commandType: CommandType.StoredProcedure)==1;
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
        /// Inserts a new generated news article with an image into the database.
        /// </summary>
        /// <param name="insertNewsWithImageDto"></param>
        /// <returns></returns>
        public async Task<bool> InsertGeneratedNewsWithImageAsync(InsertNewsWithImageDto insertNewsWithImageDto)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Title", insertNewsWithImageDto.Title, DbType.String);
                parameters.Add("@Detail", insertNewsWithImageDto.Detail, DbType.String);
                parameters.Add("@CategoryId", insertNewsWithImageDto.CategoryId, DbType.Int32);
                parameters.Add("@Language", insertNewsWithImageDto.Language.ToUpper(), DbType.String);
                parameters.Add("@IsActive", insertNewsWithImageDto.IsActive, DbType.Boolean);
                parameters.Add("@IsApproved", insertNewsWithImageDto.IsApproved, DbType.Boolean);
                parameters.Add("@Path", insertNewsWithImageDto.ImagePath, DbType.String);

                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryFirstAsync<int>("UB_sp_InsertIndividualNewsWithImage", parameters, commandType: CommandType.StoredProcedure) == 1;
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
