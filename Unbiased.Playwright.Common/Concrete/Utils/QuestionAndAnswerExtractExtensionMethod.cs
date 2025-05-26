using System.Text.Json;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

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
        public static QuestionsAndAnswersDto ExtractQuestionAndAnswer(string jsonResponse, IServiceProvider _serviceProvider, EventAndActivityLog _eventAndActivityLog)
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
            catch (Exception exception)
            {
                 _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = typeof(QuestionAndAnswerExtractExtensionMethod).FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message}",
                    EventDate = DateTime.UtcNow
                }, _serviceProvider).Wait();
                throw;
            }
            return questionsAndAnswersDto;
        }
    }
}

