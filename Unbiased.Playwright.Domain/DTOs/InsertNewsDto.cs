using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Domain.DTOs
{
    public class InsertNewsDto
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public Guid CategoryId { get; set; }
        public string Url { get; set; }
    }
}
