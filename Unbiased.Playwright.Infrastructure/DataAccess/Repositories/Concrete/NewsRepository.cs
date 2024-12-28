using Dapper;
using System.Data;
using System.Data.Common;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    /// <summary>
    /// Repository for news operations.
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="connection">The connection to the database.</param>
        public NewsRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
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
                    parameters.Add("CreatedTime", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
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

                    throw;
                }
            }
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
                                parameters.Add("Language", news.Language, DbType.String, ParameterDirection.Input);

                                var result = await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
                                if (result != 1)
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
            catch (Exception ex)
            {
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
            catch (Exception)
            {

                throw new Exception("Failed to search title: " + title);
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
            catch (Exception)
            {

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
                throw;
            }

        }

        /// <summary>
        /// Adds a generated news item to the database.
        /// </summary>
        /// <param name="generatedNews"></param>
        /// <returns></returns>
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

                    return await connection.ExecuteAsync("UB_sp_InsertUBNewsGenerated", parameters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the process value of a news item to true in the database.
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
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
            catch (Exception)
            {

                throw;
            }
        }

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
            catch (Exception)
            {

                throw;
            }
        } 
    }
}
