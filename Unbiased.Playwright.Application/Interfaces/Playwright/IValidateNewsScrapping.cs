using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Application.Interfaces.Playwright
{
    public interface IValidateNewsScrapping
    {
        Task<bool> UB_sp_UrlValidateForSearchWithTitleAsync(string title);
    }
}
