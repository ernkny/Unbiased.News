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
        public GptApiExternalService(HttpClient httpClient, IConfiguration configuration, IMediator mediator)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="mediator">The mediator instance.</param>
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
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="mediator">The mediator instance.</param>
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
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="DetailIOfNews"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="DetailIOfNews"></param>
        /// <returns></returns>
        private async Task<string> TurkishPromptMessage(string DetailIOfNews)
        {
            var newsAnalysis = $@"Bir gazeteci gibi bu haber metnini oku ve yeni bir haber olarak analiz et. İlk cümle başlık olacak şekilde içerik oluştur ve bunu sanki kendi abonelerin okuyacakmış gibi hazırla. Aynı zamanda haberi analiz edip, kendi yorumunu da ekle. Yazının yapay zeka tarafından incelenip analiz edildiğini belirt. Api cevabının bana JSON format olarak ver çünkü Kendimin oluşturduğu API den senin cevabını okuyacağım. Analiz Edilecek Haber içeriği='{DetailIOfNews}'";

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
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="DetailIOfNews"></param>
        /// <returns></returns>
        private async Task<string> EnglishPromptMessage(string DetailIOfNews)
        {
            var newsAnalysis = $@"Read this news text like a journalist and analyze it as a new article. Create the content with the first sentence as the title, and prepare it as if it were for your own subscribers. Also, analyze the news and add your own commentary. Mention that the article has been analyzed and reviewed by artificial intelligence. Start your response with 'Title:' and continue with 'Detail:', Your api response has to be JSON formatted because I will read your article from an API. '{DetailIOfNews}'";

            var prompt = $@"
            {newsAnalysis}

            This json format response type is very important!! not incluede any more than this. Please respond in the following format:

            {{ 
                ""Title"": ""[News headline]"", 
                ""Detail"": ""[News detail and analysis or your commantary]"",
            }} 
            Dont put them '```json\n";

            return await Task.FromResult(prompt);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="DetailIOfNews"></param>
        /// <returns></returns>
        private async Task<string> HoroscopePromptMessage(string horoscope)
        {
            var prompt = $@"Bir astroloji uzmanı gibi {DateTime.UtcNow.ToString("dd/MM/yyyy").ToString()} için günlük {horoscope} burcunun yorumunu yap. sadece cevabını yaz, açıklayıcı ve uzun yaz. Aynı zamanda buçların " ;


            return await Task.FromResult(prompt);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="GptApiExternalService"/> class.
        /// </summary>
        /// <param name="DetailIOfNews"></param>
        /// <returns></returns>
        private async Task<string> DailyInformationPromptMessage()
        {
            var prompt = $@"Bir tarih uzmanı gibi '{DateTime.UtcNow.ToString("dd/MM").ToString()}' için günün hem türkiye hemde dünya tarihi açısından önemli gelişmelerini yaz. sadece cevabını yaz, açıklayıcı ve uzun yaz. Profesyonel türkçe kullan";


            return await Task.FromResult(prompt);
        }
    }
}

