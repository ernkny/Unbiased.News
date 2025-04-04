using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unbiased.Playwright.Domain.DTOs
{
    public class QuestionsAndAnswersDto
    {
        public IEnumerable<QuestionAndAnswer> questions { get; set; }

        public class QuestionAndAnswer
        {
            public string question { get; set; }
            public string answer { get; set; }
        }
    }
}
