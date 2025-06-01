using Dapper;
using System.Data;
using System.Text.Json;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Connections;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Implementation of the IContentRepository interface that provides data access
    /// to content-related database operations using Dapper.
    /// </summary>
    public class ContentRepository : IContentRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public ContentRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// Executes a stored procedure to get the latest daily content.
        /// </summary>
        /// <returns>The latest content object.</returns>
        public async Task<Contents> GetLastContentAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryFirstAsync<Contents>($"Exec UB_sp_GetLastDailyContent");
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
        /// Retrieves the most recent daily horoscope information for all zodiac signs.
        /// Executes a stored procedure to get the latest horoscope data.
        /// </summary>
        /// <returns>A collection of daily horoscope details.</returns>
        public async Task<IEnumerable<HoroscopeDailyDetail>> GetDailyLastHoroscopesAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<HoroscopeDailyDetail>($"Exec UB_sp_GetDailyLastHoroscopes");
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
        /// Retrieves a paginated collection of content subheadings for a specific category.
        /// Uses a stored procedure with category ID and page number parameters.
        /// </summary>
        /// <param name="categoryId">The ID of the category to retrieve subheadings for.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <returns>A collection of content subheadings for the specified category.</returns>
        public async Task<IEnumerable<ContentSubHeading>> ContentSubHeadingsAsync(int categoryId, int pageNumber)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CategoryId", categoryId, DbType.Int32);
                    parameters.Add("@pageNumber", pageNumber, DbType.Int32);
                    var result = await connection.QueryAsync<ContentSubHeading>($"UB_sp_GetAllSubheadings", parameters, commandType: CommandType.StoredProcedure);
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
        /// Gets the total count of subheadings available for a specific category.
        /// Uses a stored procedure with a category ID parameter.
        /// </summary>
        /// <param name="categoryId">The ID of the category to count subheadings for.</param>
        /// <returns>The total number of subheadings in the category.</returns>
        public async Task<int> ContentSubHeadingsCountAsync(int categoryId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CategoryId", categoryId, DbType.Int32);
                    var result = await connection.QueryFirstAsync<int>($"UB_sp_GetAllSubheadingsCount", parameters, commandType: CommandType.StoredProcedure);
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
        ///  Retrieves all content subheadings with associated images for the home page.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentSubHeadingWithImageDto>> GetAllContentWithImageForHomePageAsync(string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@language", language, DbType.String);
                    var result = await connection.QueryAsync<ContentSubHeadingWithImageDto>($"UB_sp_GetAllContentSubheadigsWithCategoriesForHomePage", parameters, commandType: CommandType.StoredProcedure);
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
        /// Retrieves generated content details by its unique URL.
        /// Uses a stored procedure to get the raw data and deserializes JSON fields into structured objects.
        /// </summary>
        /// <param name="uniqUrl">The unique URL identifier for the content.</param>
        /// <returns>The content details associated with the URL, or null if not found.</returns>
        public async Task<GeneratedContentDto> GetGeneratedContentByUrlAsync(string uniqUrl)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UniqUrl", uniqUrl, DbType.String);

                    var resultJson = await connection.QueryFirstOrDefaultAsync<GeneratedContentRawDto>(
                        "UB_sp_GetGeneratedContentWithUniqPath",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (resultJson == null)
                        return null;

                    return new GeneratedContentDto
                    {
                        SubHeadingId = resultJson.SubHeadingId,
                        SubHeadingTitle = resultJson.SubHeadingTitle,
                        IsActive = resultJson.IsActive,
                        ContentCategoryId = resultJson.ContentCategoryId,
                        IsProccessed = resultJson.IsProccessed,
                        CreatedTime = resultJson.CreatedTime,
                        UniqUrlPath = resultJson.UniqUrlPath,
                        ContentDetail = JsonSerializer.Deserialize<ContentDetailDto>(resultJson.ContentDetail.ToString()),
                        QuestionsAndAnswers = JsonSerializer.Deserialize<List<QuestionDto>>(resultJson.QuestionsAndAnswers.ToString()),
                        Steps = JsonSerializer.Deserialize<List<StepDto>>(resultJson.Steps.ToString())
                    };
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
