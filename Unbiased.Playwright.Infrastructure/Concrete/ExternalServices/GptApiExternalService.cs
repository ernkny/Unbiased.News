using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using Unbiased.Playwright.Common.Concrete.Utils;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;

namespace Unbiased.Playwright.Infrastructure.Concrete.ExternalServices
{
    /// <summary>
    /// External service for interacting with the GPT API.
    /// </summary>
    public sealed class GptApiExternalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the GptApiExternalService class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance for making API requests.</param>
        /// <param name="configuration">The configuration instance for API URLs and keys.</param>
        /// <param name="mediator">The mediator instance for handling commands and queries.</param>
        public GptApiExternalService(HttpClient httpClient, IConfiguration configuration, IMediator mediator)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// Sends combined news details to GPT API and extracts structured news content.
        /// </summary>
        /// <param name="DetailIOfNews">The detailed content of the news to be processed.</param>
        /// <param name="language">The language enum specifying the output language.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A NewsExtractDto containing the extracted and structured news content.</returns>
        public async Task<NewsExtractDto> SendCombinedNewsDetailToGpt(string DetailIOfNews, LanguageEnums language, CancellationToken cancellationToken)
        {
            var result = new NewsExtractDto();
            var prompt = await SetPromptMessageLanguageWithDetailOfNews(language, DetailIOfNews);

            try
            {
                var response = await SendPromtToGptAndGetResponse(prompt, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    throw new Exception($"API returned error: {response.StatusCode}");
                }
                if (response.IsSuccessStatusCode)
                {
                    await _mediator.Send(new AddOpenApiResponseCommand(await response.Content.ReadAsStringAsync()));
                    result = NewsExtractExtensionMethod.ExtractNewsDetails(await response.Content.ReadAsStringAsync());

                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends a horoscope prompt to GPT API and retrieves the generated horoscope content.
        /// </summary>
        /// <param name="horoscope">The name of the horoscope sign.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A string containing the generated horoscope content.</returns>
        public async Task<string> SendHoroscopeToGptAndGetResponse(string horoscope, CancellationToken cancellationToken)
        {
            var prompt = await HoroscopePromptMessage(horoscope);

            try
            {
                var response = await SendPromtToGptAndGetResponse(prompt, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    throw new Exception($"API returned error: {response.StatusCode}");
                }
                if (response.IsSuccessStatusCode)
                {
                    await _mediator.Send(new AddOpenApiResponseCommand(await response.Content.ReadAsStringAsync()));
                   

                }
                var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                   return message.GetProperty("content").GetString();
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends a request to GPT API to generate daily information content.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A string containing the generated daily information content.</returns>
        public async Task<string> SendDailyInformationToGptAndGetResponse(CancellationToken cancellationToken)
        {
            var prompt = await DailyInformationPromptMessage();

            try
            {
                var response = await SendPromtToGptAndGetResponse(prompt, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    throw new Exception($"API returned error: {response.StatusCode}");
                }
                if (response.IsSuccessStatusCode)
                {
                    await _mediator.Send(new AddOpenApiResponseCommand(await response.Content.ReadAsStringAsync()));


                }
                var jsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var root = jsonDoc.RootElement;
                var choices = root.GetProperty("choices");
                foreach (var choice in choices.EnumerateArray())
                {
                    var message = choice.GetProperty("message");
                    return message.GetProperty("content").GetString();
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends news content to GPT API to generate questions and answers related to the news.
        /// </summary>
        /// <param name="detailOfNew">The detailed content of the news to generate questions and answers for.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A QuestionsAndAnswersDto containing the generated questions and answers.</returns>
        public async Task<QuestionsAndAnswersDto> SendNewsQuestionsAndAnswersPrompt(string detailOfNew, CancellationToken cancellationToken)
        {
            var prompt = await NewsQuestionsAndAnswersPrompt(detailOfNew);
            var result = new QuestionsAndAnswersDto();
            try
            {
                var response = await SendPromtToGptAndGetResponse(prompt, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    throw new Exception($"API returned error: {response.StatusCode}");
                }
                if (response.IsSuccessStatusCode)
                {
                    await _mediator.Send(new AddOpenApiResponseCommand(await response.Content.ReadAsStringAsync()));
                    result = QuestionAndAnswerExtractExtensionMethod.ExtractQuestionAndAnswer(await response.Content.ReadAsStringAsync());

                }

                string responseContent = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sends the provided prompt to the GPT API and returns the response.
        /// </summary>
        /// <param name="prompt">The prompt to send to the GPT API.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An HttpResponseMessage containing the API response.</returns>
        private async Task<HttpResponseMessage> SendPromtToGptAndGetResponse(string prompt, CancellationToken cancellationToken)
        {
            try
            {
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;
                var requestData = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                    new { role = "user", content = prompt }
                }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                if (cancellationToken.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sets the prompt message language based on the specified language enum and combines it with the news details.
        /// </summary>
        /// <param name="language">The language enum specifying the output language.</param>
        /// <param name="DetailIOfNews">The detailed content of the news to be processed.</param>
        /// <returns>A string containing the complete prompt message in the specified language.</returns>
        private async Task<string> SetPromptMessageLanguageWithDetailOfNews(LanguageEnums language, string DetailIOfNews)
        {
            switch (language)
            {
                case LanguageEnums.EN:
                    return await EnglishPromptMessage(DetailIOfNews);
                case LanguageEnums.TR:
                    return await TurkishPromptMessage(DetailIOfNews);
                default: throw new Exception("Language not supported");

            }
        }

        /// <summary>
        /// Creates a prompt message in Turkish for processing news content.
        /// </summary>
        /// <param name="DetailIOfNews">The detailed content of the news to be processed.</param>
        /// <returns>A string containing the Turkish prompt message.</returns>
        private async Task<string> TurkishPromptMessage(string DetailIOfNews)
        {
            var newsAnalysis = $@"Bir gazeteci gibi bu haber metnini oku ve yeni bir haber olarak analiz et. İlk cümle başlık olacak şekilde içerik oluştur ve bunu sanki kendi abonelerin okuyacakmış gibi hazırla. Aynı zamanda haberi analiz edip, kendi yorumunu da ekle. Yazının yapay zeka tarafından incelenip analiz edildiğini belirt. Api cevabının bana JSON format olarak ver çünkü Kendimin oluşturduğu API den senin cevabını okuyacağım.Yazı içeriğini olabildiğince uzun yaz. Analiz Edilecek Haber içeriği='{DetailIOfNews}'";

            var prompt = $@"
            {newsAnalysis}

            KESINLIKLE yanıtını aşağıdaki formatta ver:

            {{
                ""Title"": ""[Haber başlığı]"",
                ""Detail"": ""[Haber detayı ve analizi]""
            }} bu dönüş formatı çok önemli buna dikkat et!!
            asla bunu kullanma '```json\n";

            return await Task.FromResult(prompt);
        }

        /// <summary>
        /// Creates a prompt message in English for processing news content.
        /// </summary>
        /// <param name="DetailIOfNews">The detailed content of the news to be processed.</param>
        /// <returns>A string containing the English prompt message.</returns>
        private async Task<string> EnglishPromptMessage(string DetailIOfNews)
        {
            var newsAnalysis = $@"Read this news text like a journalist and analyze it as a new article. Create the content with the first sentence as the title, and prepare it as if it were for your own subscribers. Also, analyze the news and add your own commentary. Mention that the article has been analyzed and reviewed by artificial intelligence. Write the text content as long as possible. Also when you read finish give bias score. How biased and judgmental the news you read is on average. Bias Score is 0 to 100 Also give Reason of bias caused. Start your response with 'Title:' and continue with 'Detail:' And 'BiasScore:','BiasScoreExplanation:', Your api response has to be JSON formatted because I will read your article from an API.  '{DetailIOfNews}'";

            var prompt = $@"
            {newsAnalysis}

            This json format response type is very important!! not incluede any more than this. Please respond in the following format:

            {{ 
                ""Title"": ""[News headline]"", 
                ""Detail"": ""[News detail and analysis or your commantary]"",
                ""BiasScore"": ""[Give your bias score to all readed news]"",
                ""BiasScoreExplanation"": ""[Give your bias score Explanation]"",
            }} 
           essipacially Dont put  '```json\n";

            return await Task.FromResult(prompt);
        }

        /// <summary>
        /// Creates a prompt message for generating horoscope content.
        /// </summary>
        /// <param name="horoscope">The name of the horoscope sign.</param>
        /// <returns>A string containing the horoscope prompt message.</returns>
        private async Task<string> HoroscopePromptMessage(string horoscope)
        {
            var prompt = $@"Bir astroloji uzmanı gibi {DateTime.UtcNow.ToString("dd/MM/yyyy").ToString()} tarihi için günlük {horoscope} burcunun yorumunu yap. sadece cevabını yaz, açıklayıcı ve orta uzunlukta yaz.Profesyonel türkçe kullan." ;


            return await Task.FromResult(prompt);
        }


        /// <summary>
        /// Creates a prompt message for generating daily information content.
        /// </summary>
        /// <returns>A string containing the daily information prompt message.</returns>
        private async Task<string> DailyInformationPromptMessage()
        {
            var prompt = $@"Bir tarih uzmanı gibi '{DateTime.UtcNow.ToString("dd/MM").ToString()}' için günün hem türkiye hemde dünya tarihi açısından önemli gelişmelerini yaz. sadece cevabını yaz, açıklayıcı ve uzun yaz. Profesyonel türkçe kullan";


            return await Task.FromResult(prompt);
        }

        /// <summary>
        /// Creates a prompt message for generating questions and answers related to news content.
        /// </summary>
        /// <param name="detailOfNew">The detailed content of the news to generate questions and answers for.</param>
        /// <returns>A string containing the questions and answers prompt message.</returns>
        private async Task<string> NewsQuestionsAndAnswersPrompt(string detailOfNew)
        {
            var prompt = $@"'{detailOfNew}'Can you read this news and come up with 3-5 questions that we need to question? Can you collect the questions under questions in json format? And give these question answer as json to me as [questions{{ [question:'',answer:''] }}] I will read them from the API. I will read them from the API. IMPORTANT:Just reply in a text format converted to json format";
                 return await Task.FromResult(prompt);
        }
    }
}

