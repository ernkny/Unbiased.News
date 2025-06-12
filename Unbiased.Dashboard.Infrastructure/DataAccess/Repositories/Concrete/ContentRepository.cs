using Dapper;
using System.Data;
using System.Text.Json;
using Unbiased.Dashboard.Domain.Dto_s.Content;
using Unbiased.Dashboard.Infrastructure.DataAccess.Connections;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Provides methods for managing and retrieving content subheadings, categories, and generated content.
    /// </summary>
    public class ContentRepository : IContentRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        public ContentRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieves all content subheadings for the dashboard with pagination and filtering options.
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="Language"></param>
        /// <param name="CategoryId"></param>
        /// <param name="IsProcess"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentSubheadingDto>> GetAllContentsAsync(int PageNumber, int PageSize, string Language, int? CategoryId, bool? IsProcess)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@pageNumber", PageNumber);
                    parameters.Add("@pageSize", PageSize);
                    parameters.Add("@language", Language);
                    parameters.Add("@categoryid", CategoryId);
                    parameters.Add("@IsProcess", IsProcess);
                    var result = await connection.QueryAsync<ContentSubheadingDto>($"UB_sp_GetAllContentSubheadingForDashboard", parameters, commandType: System.Data.CommandType.StoredProcedure);
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
        ///  Retrieves the count of all content subheadings based on the specified parameters such as language, category ID, and processing status.
        /// </summary>
        /// <param name="Language"></param>
        /// <param name="CategoryId"></param>
        /// <param name="IsProcess"></param>
        /// <returns></returns>
        public async Task<int> GetAllContentsCountAsync(string Language, int? CategoryId, bool? IsProcess)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@language", Language);
                    parameters.Add("@categoryid", CategoryId);
                    parameters.Add("@IsProcess", IsProcess);
                    var result = await connection.QueryFirstAsync<int>($"UB_sp_GetAllContentSubheadingCountForDashboard", parameters, commandType: System.Data.CommandType.StoredProcedure);
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
        ///   Retrieves all content categories from the database.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ContentCategories>> GetAllContentCategoriesAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var result = await connection.QueryAsync<ContentCategories>($"UB_sp_GetAllContentCategories", commandType: System.Data.CommandType.StoredProcedure);
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
        ///  Retrieves the generated content by its unique identifier.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<GeneratedContentDto> GetGeneratedContentByIdAsync(int Id)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Id", Id, DbType.Int32);

                    var resultJson = await connection.QueryFirstOrDefaultAsync<GeneratedContentRawDto>(
                        "UB_sp_GetGeneratedContentDetailForDashboard",
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
