using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.News.Domain.DTOs
{
    public class HomePageCategorieSliderWithCountDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int NewsCount { get; set; }
        public Guid IdForNews { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
    }
}
