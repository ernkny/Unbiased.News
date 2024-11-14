using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;

namespace Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract
{
    public interface INewsImageRepository
    {
        Task<Guid> AddNewsImageAsync(InsertNewsImageDto addNewsImageDto);
        Task<IEnumerable<string>> GetNewsWithoutImages(DateTime startDate);
    }
}
