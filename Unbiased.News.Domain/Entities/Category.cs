using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.News.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public int OrderLevel { get; set; }
    }
}
