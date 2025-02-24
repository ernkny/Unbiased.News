using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Dashboard.Domain.Dto_s
{
    public class InsertNewsWithImageDto
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int? CategoryId { get; set; } = null;
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string ImagePath { get; set; }
    }
}
