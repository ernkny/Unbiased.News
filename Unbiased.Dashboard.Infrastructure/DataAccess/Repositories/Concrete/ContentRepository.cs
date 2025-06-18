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
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="connection">The Unbiased SQL connection.</param>
        /// <param name="eventAndActivityLog">The event and activity logging service.</param>
        public ContentRepository(UnbiasedSqlConnection connection, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves all content subheadings for the dashboard with pagination and filtering options.
        /// </summary>
        /// <param name="PageNumber">The page number for pagination.</param>
        /// <param name="PageSize">The number of items per page.</param>
        /// <param name="Language">The language filter for content.</param>
        /// <param name="CategoryId">The optional category identifier to filter by.</param>
        /// <param name="IsProcess">The optional processing status filter.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of content subheading DTOs.</returns>
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
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves the count of all content subheadings based on the specified parameters such as language, category ID, and processing status.
        /// </summary>
        /// <param name="Language">The language filter for content.</param>
        /// <param name="CategoryId">The optional category identifier to filter by.</param>
        /// <param name="IsProcess">The optional processing status filter.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the total count of content subheadings.</returns>
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
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves all content categories from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the collection of content categories.</returns>
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
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Retrieves the generated content by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the content to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated content DTO or null if not found.</returns>
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
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Updates generated content in the database.
        /// </summary>
        /// <param name="request">The request containing all the updated content data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateGeneratedContentAsync(UpdateAllContentDataRequest request)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ContentSubHeadingId", request.ContentSubHeadingId);
                    parameters.Add("@ContentCategoryId", request.ContentCategoryId);
                    parameters.Add("@Title", request.Title);
                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@UniqUrlPath", request.UniqUrlPath);
                    parameters.Add("@CreatedTime", request.CreatedTime);
                    parameters.Add("@SubTitle", request.SubTitle);
                    parameters.Add("@ImagePrompt", request.ImagePrompt);
                    parameters.Add("@Hashtags", request.Hashtags);
                    parameters.Add("@ImagePath", request.ImagePath);
                    parameters.Add("@DetailIsActive", request.DetailIsActive ?? true);
                    parameters.Add("@DetailIsDeleted", request.DetailIsDeleted ?? false);
                    parameters.Add("@QuestionsAndAnswers", CreateQuestionsAndAnswersDataTable(request.Questions).AsTableValuedParameter("ContentQuestionsAndAnswersType"));
                    parameters.Add("@Steps", CreateStepsDataTable(request.Steps).AsTableValuedParameter("ContentStepsType"));

                    await connection.ExecuteAsync(
                        "UB_sp_UpdateAllContentData",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return true;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"Content Update Error: {exception.Message}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        private DataTable CreateQuestionsAndAnswersDataTable(List<QuestionDto> questions)
        {
            var table = new DataTable();
            table.Columns.Add("Question", typeof(string));
            table.Columns.Add("Answer", typeof(string));

            if (questions != null)
            {
                foreach (var q in questions)
                {
                    table.Rows.Add(q.Question, q.Answer);
                }
            }

            return table;
        }

        private DataTable CreateStepsDataTable(List<StepDto> steps)
        {
            var table = new DataTable();

            table.Columns.Add("StepNumber", typeof(int));
            table.Columns.Add("StepTitle", typeof(string));
            table.Columns.Add("StepDescription", typeof(string));

            if (steps != null)
            {
                foreach (var step in steps)
                {
                    table.Rows.Add(step.StepNumber, step.StepTitle, step.StepDescription);
                }
            }

            return table;
        }
    }
}
