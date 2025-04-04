using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object for displaying category information with news counts in the home page slider.
    /// </summary>
    public class HomePageCategorieSliderWithCountDto
    {
        /// <summary>
        /// The unique identifier of the category.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string CategoryName { get; set; }
        
        /// <summary>
        /// The count of news items in this category.
        /// </summary>
        public int NewsCount { get; set; }
        
        /// <summary>
        /// The unique identifier of a featured news item in this category.
        /// </summary>
        public Guid IdForNews { get; set; }
        
        /// <summary>
        /// The title of the featured news item.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The file path to the image associated with the featured news item.
        /// </summary>
        public string Path { get; set; }
    }
}
