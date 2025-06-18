using Dapper;
using System.Data;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    public class SearchUrlRepository : ISearchUrlRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchUrlRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public SearchUrlRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all active keywords for search.
        /// </summary>
        /// <returns>A list of active keywords for search.</returns>
        public async Task<IEnumerable<ActiveUrlsForSearchDto>> GetAllActiveUrlsForSearchAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {
                    return await connection.QueryAsync<ActiveUrlsForSearchDto>("UB_sp_GetAllActiveUrls", commandType: CommandType.StoredProcedure);
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

        /// <summary>
        /// Retrieves all active keywords for search.
        /// </summary>
        /// <returns>A list of active keywords for search.</returns>
        public async Task<bool> UpdateAllSearhcUrlNextRunTimeAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {

                    return await connection.ExecuteAsync("UB_sp_UpdateSearchUrlNextRun", commandType: CommandType.StoredProcedure) == 1;
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
}
