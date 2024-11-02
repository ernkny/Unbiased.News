using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Domain.Entities
{
    public class News
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedUser { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Url { get; set; }
        public Guid MatchId { get; set; }
        public bool IsProcessed { get; set; }
    }
}
