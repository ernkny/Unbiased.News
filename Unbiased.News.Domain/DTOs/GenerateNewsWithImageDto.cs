using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.News.Domain.DTOs
{
    public  class GenerateNewsWithImageDto
    { 

        public Guid Id { get; set; }

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
        /// Match ID associated with the generated news.
        /// </summary>
        public string MatchId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Langauge { get; set; }

        public string Path { get; set; }
    }
}
