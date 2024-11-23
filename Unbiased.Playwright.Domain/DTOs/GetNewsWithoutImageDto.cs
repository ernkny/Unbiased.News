using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Domain.DTOs
{
    public class GetNewsWithoutImageDto
    {
        public string MatchId { get; set; }
        public string Title { get; set; }
    }
}
