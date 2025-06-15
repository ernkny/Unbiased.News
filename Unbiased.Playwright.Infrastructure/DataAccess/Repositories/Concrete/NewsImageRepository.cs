using Dapper;
using System.Data;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository for news image related operations.
    /// </summary>
    public class NewsImageRepository : INewsImageRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsImageRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public NewsImageRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Adds a new news image to the database.
        /// </summary>
        /// <param name="addNewsImageDto">The news image to add.</param>
        /// <returns>The ID of the newly added news image.</returns>
        public async Task<bool> AddNewsImageAsync(InsertNewsImageDto addNewsImageDto)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var guid = Guid.NewGuid();
                    var parameters = new DynamicParameters();
                    parameters.Add("id", Guid.NewGuid(), DbType.Guid);
                    parameters.Add("matchId", addNewsImageDto.MatchId, DbType.String);
                    parameters.Add("path", addNewsImageDto.filePath, DbType.String);

                    var result = await connection.ExecuteAsync("UB_sp_InsertNewsImage", parameters, commandType: CommandType.StoredProcedure);

                    return result == 1;
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
        ///  Retrieves all generated news that do not have an image.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<GeneratedNewsWithNoneImageDto>> GenerateImagesWithNoneHasGeneratedAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {

                    return await connection.QueryAsync<GeneratedNewsWithNoneImageDto>("UB_sp_GetAllGeneratedNewsWithNoneImage", commandType: CommandType.StoredProcedure);
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
        /// Retrieves all news images that do not have an image.
        /// </summary>
        /// <param name="startDate">The start date for filtering.</param>
        /// <returns>A list of news images without images.</returns>
        public async Task<IEnumerable<GetNewsWithoutImageDto>> GetNewsWithoutImagesAsync(DateTime startDate)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("startDate", startDate, DbType.DateTime);

                    return await connection.QueryAsync<GetNewsWithoutImageDto>("UB_sp_GetNewsWithoutImages", parameters, commandType: CommandType.StoredProcedure);
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
        ///  Validates if a news image exists for a given match ID.
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public async Task<bool> GetNewsImageWithMatchIdAsync(string matchId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("matchId", matchId, DbType.String);
                    return await connection.QueryFirstAsync<int>("UB_sp_ValidateNewsImage", parameters, commandType: CommandType.StoredProcedure) == 1;

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
