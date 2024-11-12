using Dapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
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

        public async Task<IEnumerable<News>> GetAllNewsByNotIncludedProcessAsync()
        {
            using (var connection = _connection.CreateConnection())
            {
                return await connection.QueryAsync<News>("UB_sp_GetAllNewsNotIncludedProcess", commandType: CommandType.StoredProcedure);

            }
        }

        public async Task<IEnumerable<string>> GetAllActiveKeywordsForSearchAsync()
        {
            using (var connection =_connection.CreateConnection())
            {
                return await connection.QueryAsync<string>("UB_sp_GetAllActiveKeywords", commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> AddRangeAllNews(IEnumerable<News> listOfNews)
        {
            var result = 0;
            using (var connection = _connection.CreateConnection())
            {
                foreach (var news in listOfNews)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Title", news.Title, DbType.String, ParameterDirection.Input);
                    parameters.Add("Detail", news.Detail, DbType.String, ParameterDirection.Input);
                    parameters.Add("MatchId", news.MatchId, DbType.String, ParameterDirection.Input);
                    parameters.Add("CategoryId", news.CategoryId, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("CreatedTime", news.CreatedTime, DbType.DateTime, ParameterDirection.Input);
                    parameters.Add("IsActive", news.IsActive, DbType.Boolean, ParameterDirection.Input);
                    parameters.Add("IsDeleted", news.IsDeleted, DbType.Boolean, ParameterDirection.Input);
                    parameters.Add("Url", news.Url, DbType.String, ParameterDirection.Input);
                    parameters.Add("IsProcessed", false, DbType.Boolean, ParameterDirection.Input);

                    result=await connection.ExecuteAsync("UB_sp_InsertUBNews", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            return await Task.FromResult(result==1);
        }
    }
}
