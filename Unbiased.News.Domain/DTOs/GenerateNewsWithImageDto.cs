using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.News.Domain.DTOs
{
    /// <summary>
    /// Data transfer object representing a news item with its associated image information.
    /// Used for transferring complete news data including image paths between application layers.
    /// </summary>
    public  class GenerateNewsWithImageDto
    { 
        /// <summary>
        /// Unique identifier for the news item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the news item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detailed content of the generated news.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Foreign key referencing the Category entity.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Foreign key referencing the Category entity.
        /// </summary>
        public string CategoryName{ get; set; }

        /// <summary>
        /// Match ID associated with the generated news.
        /// </summary>
        public string MatchId { get; set; }

        /// <summary>
        /// Date and time when the news item was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Language of the news content.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// File path to the image associated with the news.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Unique URL path for accessing the news item.
        /// </summary>
        public string UniqUrlPath { get; set; }

        /// <summary>
        /// Bias score of the news content, indicating the level of bias.
        /// </summary>
        public string BiasScore { get; set; }

        /// <summary>
        /// Explanation of how the bias score was determined.
        /// </summary>
        public string BiasScoreExplanation { get; set; }

        /// <summary>
        /// Number of sources used to generate the news content.
        /// </summary>
        public int SourceCount { get; set; }
    }
}
