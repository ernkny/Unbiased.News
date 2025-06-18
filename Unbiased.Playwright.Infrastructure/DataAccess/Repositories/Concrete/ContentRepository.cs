using Dapper;
using System.Data;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Implementation of the IContentRepository interface that provides data access
    /// </summary>
    public class ContentRepository : IContentRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IEventAndActivityLog _eventAndActivityLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public ContentRepository(UnbiasedSqlConnection connection, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="horoscopeDetail"></param>
        /// <returns></returns>
        public async Task<bool> AddDailyHoroscopeAsync(HoroscopeDailyDetail horoscopeDetail)
        {
            try
            {
                var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                var turkeyTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Detail", horoscopeDetail.Detail, DbType.String, ParameterDirection.Input);
                    parameters.Add("@HoroscopeId", horoscopeDetail.HoroscopeId, DbType.String, ParameterDirection.Input);
                    parameters.Add("@CreatedDate", turkeyTime, DbType.DateTime);
                    var result = await connection.ExecuteAsync("UB_sp_InsertHoroscopeDetail", parameters, commandType: CommandType.StoredProcedure);
                    return result == 1;
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
        /// Inserts daily content information into the database.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<bool> AddDailyContentInformationAsync(Contents content)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ContentDetail", content.ContentDetail, DbType.String, ParameterDirection.Input);
                    parameters.Add("@ContentCategoryId", content.ContentCategoryId, DbType.String, ParameterDirection.Input);
                    parameters.Add("@CreatedDate", content.CreatedDate, DbType.DateTime);
                    var result = await connection.ExecuteAsync("UB_sp_InsertContentDetail", parameters, commandType: CommandType.StoredProcedure);
                    return result == 1;
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
        /// Inserts generated content into the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddGeneratedContentAsync(InsertAllContentDataRequest request)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ContentCategoryId", request.ContentCategoryId);
                    parameters.Add("@ContentSubHeadingId", request.ContentSubHeadingId);
                    parameters.Add("@Title", request.Title);
                    parameters.Add("@IsActive", request.IsActive);
                    parameters.Add("@SubTitle", request.SubTitle);
                    parameters.Add("@ImagePrompt", request.ImagePrompt);
                    parameters.Add("@Hashtags", request.Hashtags);
                    parameters.Add("@DetailIsActive", request.DetailIsActive);
                    parameters.Add("@DetailIsDeleted", request.DetailIsDeleted);
                    parameters.Add("@ImagePath", request.ImagePath);
                    parameters.Add("@QuestionsAndAnswers", CreateQuestionsAndAnswersDataTable(request.Questions).AsTableValuedParameter("ContentQuestionsAndAnswersType"));
                    parameters.Add("@Steps", CreateStepsDataTable(request.Steps).AsTableValuedParameter("ContentStepsType"));

                    await connection.ExecuteAsync(
                        "UB_sp_InsertAllContentData",
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
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }

        /// <summary>
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ContentSubHeading>> GetAllNoneGeneratedSubHeadingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {

                    return await connection.QueryAsync<ContentSubHeading>(
                         "UB_sp_GetAllNoneGeneratedContentSubheadings",
                         commandType: CommandType.StoredProcedure
                     );
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
        /// Inserts content subheadings into the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<bool> InsertContentSubheadings(int id, string title)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ContentCategoryId", id);
                    parameters.Add("@Title", title);
                    var result = await connection.ExecuteAsync(
                        "UB_sp_InsertContentSubheading",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return result == 1;
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
        /// <returns></returns>
        public async Task<IEnumerable<ContentCategory>> GetAllContentCategories()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    return await connection.QueryAsync<ContentCategory>(
                        "UB_sp_GetAllContentCategories",
                        commandType: CommandType.StoredProcedure
                    );
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
        /// Retrieves the most recent content entry from the database.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable CreateQuestionsAndAnswersDataTable(List<ContentQuestionAndAnswer> list)
        {
            try
            {
                var table = new DataTable();
                table.Columns.Add("Question", typeof(string));
                table.Columns.Add("Answer", typeof(string));

                foreach (var item in list)
                {
                    table.Rows.Add(item.Question, item.Answer);
                }

                return table;
            }
            catch (Exception exception)
            {
                _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                }).Wait();
                throw;
            }

        }

        /// <summary>
        /// Creates a DataTable for content steps.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable CreateStepsDataTable(List<ContentStep> list)
        {
            try
            {
                var table = new DataTable();
                table.Columns.Add("StepNumber", typeof(int));
                table.Columns.Add("StepTitle", typeof(string));
                table.Columns.Add("StepDescription", typeof(string));

                foreach (var item in list)
                {
                    table.Rows.Add(item.StepNumber, item.StepTitle, item.StepDescription);
                }

                return table;
            }
            catch (Exception exception)
            {
                _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                }).Wait();
                throw;
            }

        }
    }
}
