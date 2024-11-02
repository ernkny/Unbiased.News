using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface INewsRepository
    {
        Task<Guid> InsertNewsAsync(InsertNewsDto addNewsDto);
    }
}
