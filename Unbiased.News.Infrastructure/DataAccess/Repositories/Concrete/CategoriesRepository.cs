using Dapper;
using System;
using System.Data;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository class for managing categories.
    /// </summary>
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public CategoriesRepository(UnbiasedSqlConnection connection, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all categories from the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of categories.</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<Category>($"Exec UB_sp_GetAllcategoriesWithCount");
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
                });
                throw;
            }

        }

        /// <summary>
        ///  Retrieves categories for the home page slider with count asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HomePageCategorieSliderWithCountDto>> GetHomePageCategorieSliderWithCountAsync(string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@language", language);
                    var result = await connection.QueryAsync<HomePageCategorieSliderWithCountDto>($"UB_sp_HomePageCategorieSliderWithCount", parameters, commandType: CommandType.StoredProcedure);
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
                });
                throw;
            }

        }

        /// <summary>
        ///  Retrieves random last generated news for home page categories asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageCategoriesRandomLastGeneratedNewsAsync(string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@language", language);
                    var result = await connection.QueryAsync<HomePageCategoriesRandomLastGeneratedNewsDto>($"UB_sp_GetRandomLastGeneratedNews", parameters, commandType: CommandType.StoredProcedure);
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
                });
                throw;
            }

        }

        /// <summary>
        ///  Retrieves top categories with last generated news for the home page asynchronously.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageTopCategoriesGeneratedNewsAsync(string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@language", language);
                    var result = await connection.QueryAsync<HomePageCategoriesRandomLastGeneratedNewsDto>($"UB_sp_GetTopCategoriesGeneratedNews", parameters, commandType: CommandType.StoredProcedure);
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
                });
                throw;
            }
        }
    }
}
