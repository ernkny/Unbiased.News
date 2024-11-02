using Dapper;
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

        public async Task<Guid> InsertNewsAsync(InsertNewsDto addNewsDto)
        {
            using (var connection = _connection.CreateConnection())
            {
                var newId = Guid.NewGuid();
                var newData = new News()
                {
                    Id = newId,
                    Title = addNewsDto.Title,
                    Detail = addNewsDto.Detail,
                    CategoryId = addNewsDto.CategoryId,
                    CreatedTime = DateTime.Now,
                    IsActive = true,
                    Url = addNewsDto.Url
                };
                await connection.ExecuteAsync(@"INSERT INTO UB_News(Id, Title, Detail, CategoryId, CreatedTime, IsActive, IsDeleted, Url, IsProcessed)
VALUES(@Id, @Title, @Detail, @CategoryId, @CreatedTime, @IsActive, @IsDeleted, @Url, @IsProcessed)", newData);
                return newId;
            }
        }
    }
}
