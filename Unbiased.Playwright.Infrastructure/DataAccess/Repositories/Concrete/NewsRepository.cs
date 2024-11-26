using Dapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Connections;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Concrete
{
    public class NewsRepository : INewsRepository
    {
        private readonly UnbiasedSqlConnection _connection;

        public NewsRepository(UnbiasedSqlConnection connection)
        {
            _connection = connection;
        }

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

                    await connection.ExecuteAsync("UB_sp_InsertUBNews", parameters, commandType: CommandType.StoredProcedure);
                    return guid;
                }
            }
            catch (Exception)
            {

                throw new Exception("Failed to insert news: " + addNewsDto.Title);
            }

        }

        public async Task<IEnumerable<News>> GetAllNewsByNotIncludedProcessAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {
                    return await connection.QueryAsync<News>("UB_sp_GetAllNewsNotIncludedProcess", commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {

                    throw new Exception("Failed to get all news not included process ");
                }
            }
        }

        public async Task<IEnumerable<string>> GetAllActiveKeywordsForSearchAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                try
                {

                    return await connection.QueryAsync<string>("UB_sp_GetAllActiveKeywords", commandType: CommandType.StoredProcedure);
                }
                catch (Exception exception)
                {

                    throw new Exception("There no keyword for search");
                }
            }
        }

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

                                var result = await connection.ExecuteAsync(query, parameters, transaction, commandType: CommandType.StoredProcedure);
                                if (result != 1)
                                {
                                    throw new Exception("Failed to insert news: " + news.Title);
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
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ValidateUrlForSearchWithTitleAsync(string title)
        {
            try
            {
                using (var connection=_connection.CreateConnection())
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

        public async Task<bool> AddOpenAiResponseAsync(string responseBody)
        {
            using (var connection=_connection.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("responseData", responseBody, DbType.String);
                return await connection.ExecuteAsync("UB_sp_InsertOpenAiResponse", parameters, commandType: CommandType.StoredProcedure)==1;
            }
        }

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
                    parameters.Add("CategoryId", 1, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("MatchId", generatedNews.MatchId, DbType.String, ParameterDirection.Input);

                  return  await connection.ExecuteAsync("UB_sp_InsertUBNewsGenerated", parameters, commandType: CommandType.StoredProcedure) == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
