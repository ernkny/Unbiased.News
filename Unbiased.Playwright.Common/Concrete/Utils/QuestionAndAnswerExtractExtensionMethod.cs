using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Unbiased.Playwright.Domain.DTOs;

namespace Unbiased.Playwright.Common.Concrete.Utils
{
    /// <summary>
    /// Provides extension methods for extracting question and answer data from a JSON response.
    /// </summary>
    public static class QuestionAndAnswerExtractExtensionMethod
    {
        /// <summary>
        /// Extracts question and answer data from a JSON response returned by an AI service.
        /// </summary>
        /// <param name="jsonResponse">The JSON response to extract question and answer data from.</param>
        /// <returns>A <see cref="QuestionsAndAnswersDto"/> containing the extracted question and answer data.</returns>
        public static QuestionsAndAnswersDto ExtractQuestionAndAnswer(string jsonResponse)
        {
            var questionsAndAnswersDto = new QuestionsAndAnswersDto();
            try
            {
                var jsonDoc = JsonDocument.Parse(jsonResponse);
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                    var content = message.GetProperty("content").GetString().Replace("```json",string.Empty).Replace("```",string.Empty);


                    return JsonSerializer.Deserialize<QuestionsAndAnswersDto>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing JSON: " + ex.Message);
            }
            return questionsAndAnswersDto;
        }
    }
}

