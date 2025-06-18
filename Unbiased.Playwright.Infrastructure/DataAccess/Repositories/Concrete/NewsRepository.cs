using Dapper;
using System.Data;
using System.Data.Common;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository implementation for news-related database operations.
    /// Provides methods for adding, retrieving, and updating news entities in the database.
    /// Uses stored procedures for efficient database interactions.
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;


        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public NewsRepository(UnbiasedSqlConnection connection, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
        }

        /// <summary>
        /// Adds a new news item to the database.
        /// </summary>
        /// <param name="addNewsDto">The news item to add.</param>
        /// <returns>The ID of the newly added news item.</returns>
        public async Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var guid = new Guid();
                    var parameters = new DynamicParameters();
                    parameters.Add("Id", guid, DbType.Guid, ParameterDirection.Input);
                    parameters.Add("Title", addNewsDto.Title, DbType.String, ParameterDirection.Input);
                    parameters.Add("Detail", addNewsDto.Detail, DbType.String, ParameterDirection.Input);
                    parameters.Add("CategoryId", addNewsDto.CategoryId, DbType.Guid, ParameterDirection.Input);
                    parameters.Add("CreatedTime", DateTime.UtcNow, DbType.DateTime, ParameterDirection.Input);
                    parameters.Add("IsActive", true, DbType.Boolean, ParameterDirection.Input);
                    parameters.Add("IsDeleted", false, DbType.Boolean, ParameterDirection.Input);
                    parameters.Add("Url", addNewsDto.Url, DbType.String, ParameterDirection.Input);
                    parameters.Add("IsProcessed", false, DbType.Boolean, ParameterDirection.Input);
                    parameters.Add("Language", addNewsDto.Language, DbType.String, ParameterDirection.Input);

                    await connection.ExecuteAsync("UB_sp_InsertUBNews", parameters, commandType: CommandType.StoredProcedure);
                    return guid;
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
        /// Retrieves all news items that have not been processed.
        /// </summary>
        /// <returns>A list of news items that have not been processed.</returns>
        public async Task<IEnumerable<News>> GetAllNewsByNotIncludedProcessAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {
                    return await connection.QueryAsync<News>("UB_sp_GetAllNewsNotIncludedProcess", commandType: CommandType.StoredProcedure);
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
        /// Adds a range of news items to the database.
        /// </summary>
        /// <param name="listOfNews">The list of news items to add.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public async Task<bool> AddRangeAllNewsAsync(IEnumerable<News> listOfNews)
        {
            if (listOfNews == null || !listOfNews.Any())
                return false;

            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    await ((DbConnection)connection).OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = "UB_sp_InsertUBNews";
                            foreach (var news in listOfNews)
                            {
                                var parameters = new DynamicParameters();
                                parameters.Add("Title", news.Title, DbType.String);
                                parameters.Add("Detail", news.Detail, DbType.String);
                                parameters.Add("MatchId", news.MatchId, DbType.String);
                                parameters.Add("CategoryId", news.CategoryId, DbType.Int32);
                                parameters.Add("CreatedTime", news.CreatedTime, DbType.DateTime);
                                parameters.Add("IsActive", news.IsActive, DbType.Boolean);
                                parameters.Add("IsDeleted", news.IsDeleted, DbType.Boolean);
                                parameters.Add("Url", news.Url, DbType.String);
                                parameters.Add("IsProcessed", false, DbType.Boolean);
                                parameters.Add("Language", news.Language, DbType.String);

                                var result = await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
                                if (result == 0)
                                {
                                    throw new Exception("Failed to insert news url: " + news.Url);
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
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
        /// Validates a URL for search with a given title.
        /// </summary>
        /// <param name="title">The title to search for.</param>
        /// <returns>True if the URL is valid, false otherwise.</returns>
        public async Task<bool> ValidateUrlForSearchWithTitleAsync(string title)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Title", title, DbType.String);
                    return await connection.QueryFirstAsync<int>("UB_sp_UrlValidateForSearchWithTitle", parameters, commandType: CommandType.StoredProcedure) == 1;
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
        /// Retrieves all combined details.
        /// </summary>
        /// <returns>A list of combined details.</returns>
        public async Task<IEnumerable<GeneratedNewsDto>> GetAllCombinedDetailsAsync()
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    return await connection.QueryAsync<GeneratedNewsDto>("UB_sp_CombinedDetails", commandType: CommandType.StoredProcedure);
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
        /// Adds an OpenAI response to the database.
        /// </summary>
        /// <param name="responseBody">The response body from OpenAI.</param>
        /// <returns>True if the response was added successfully, false otherwise.</returns>
        public async Task<bool> AddOpenAiResponseAsync(string responseBody)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("responseData", responseBody, DbType.String);
                    return await connection.ExecuteAsync("UB_sp_InsertOpenAiResponse", parameters, commandType: CommandType.StoredProcedure) == 1;
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
        /// Adds a generated news item to the database.
        /// </summary>
        /// <param name="generatedNews">The generated news entity to be added to the database.</param>
        /// <returns>True if the news was added successfully, false otherwise.</returns>
        public async Task<bool> AddGeneratedNews(News generatedNews)
        {
            if (generatedNews == null)
                return false;

            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Title", generatedNews.Title, DbType.String, ParameterDirection.Input);
                    parameters.Add("Detail", generatedNews.Detail, DbType.String, ParameterDirection.Input);
                    parameters.Add("CategoryId", generatedNews.CategoryId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("MatchId", generatedNews.MatchId, DbType.String, ParameterDirection.Input);
                    parameters.Add("Language", generatedNews.Language, DbType.String, ParameterDirection.Input);
                    parameters.Add("BiasScore", generatedNews.BiasScore, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("BiasScoreExplanation", generatedNews.BiasScoreExplanation, DbType.String, ParameterDirection.Input);
                    parameters.Add("ImagePrompt", generatedNews.ImagePrompt, DbType.String, ParameterDirection.Input);
                    parameters.Add("UniqUrlPath", generatedNews.UniqUrlPath, DbType.String, ParameterDirection.Input);

                    return await connection.ExecuteAsync("UB_sp_InsertUBNewsGenerated", parameters, commandType: CommandType.StoredProcedure) == 1;
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
        /// Updates the process status of a news item to indicate it has been processed.
        /// </summary>
        /// <param name="matchId">The match identifier of the news item to update.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public async Task<bool> UpdateNewsProcessValueAsTrueAsync(string matchId)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("MatchId", matchId, DbType.String);
                    return await connection.ExecuteAsync("UB_sp_UpdateNewsProcessValueAsTrue", parameters, commandType: CommandType.StoredProcedure) == 1;
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
        /// Retrieves  generated news items in the specified language.
        /// </summary>
        /// <param name="language">The language code of the news to retrieve (e.g., "EN" for English, "TR" for Turkish).</param>
        /// <returns>A collection of generated news entities in the specified language.</returns>
        public async Task<IEnumerable<GeneratedNews>> GetGeneratedNewsAsync(string language)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("language", language, DbType.String);
                    return await connection.QueryAsync<GeneratedNews>("UB_sp_GetAllGeneratedNews", parameters, commandType: CommandType.StoredProcedure);
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
        /// Inserts a new question and its corresponding answer into the database.
        /// </summary>
        /// <param name="QuestionAndAnswer">The DTO containing the question, answer, and related metadata.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains a boolean value indicating whether the insert operation was successful.
        /// </returns>
        public async Task<bool> InsertQuestionAndAnswerAsync(QuestionAnswerDto QuestionAndAnswer)
        {
            try
            {
                using (var connection = _connection.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Question", QuestionAndAnswer.Question, DbType.String);
                    parameters.Add("@Answer", QuestionAndAnswer.Answer, DbType.String);
                    parameters.Add("@CreatedDate", DateTime.UtcNow, DbType.String);
                    parameters.Add("@MatchId", QuestionAndAnswer.MatchId, DbType.String);
                    return await connection.ExecuteAsync("UB_sp_InsertQuestionsAndAnswers", parameters, commandType: CommandType.StoredProcedure) == 1;
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
