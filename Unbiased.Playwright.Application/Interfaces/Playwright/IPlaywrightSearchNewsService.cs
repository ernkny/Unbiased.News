using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Application.Interfaces.Playwright
{
    public interface IPlaywrightSearchNewsService
    {
        Task<bool> SearchNewsWithKeywordAndSave();
    }
}
