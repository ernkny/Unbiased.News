using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Application.Interfaces
{
    public interface INewsService
    {
        Task<Guid> AddNewsAsync(InsertNewsDto addNewsDto);
    }
}
