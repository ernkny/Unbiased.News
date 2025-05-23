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

            try
            {
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;

                var prompt = language == LanguageEnums.TR
                    ? await TurkishPromptMessage(DetailIOfNews)
                    : await EnglishPromptMessage(DetailIOfNews);

                var requestData = new
                {
                    model = "gpt-4o-mini",
                    messages = new object[]
                    {
                new
                {
                    role = "developer",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = prompt
                        }
                    }
                },
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = DetailIOfNews
                        }
                    }
                }
                    },
                    response_format = new
                    {
                        type = "json_schema",
                        json_schema = new
                        {
                            name = "generate_news_article",
                            strict = false,
                            schema = new
                            {
                                type = "object",
                                properties = new
                                {
                                    Title = new
                                    {
                                        type = "string",
                                        description = "A short, attractive title summarizing the news article."
                                    },
                                    Detail = new
                                    {
                                        type = "string",
                                        description = "A detailed, formatted (using HTML tags) news article including commentary and original source analysis."
                                    },
                                    BiasScore = new
                                    {
                                        type = "string",
                                        description = "Bias evaluation score between 0 (unbiased) and 100 (very biased).",
                                        minimum = 0,
                                        maximum = 100
                                    },
                                    BiasScoreExplanation = new
                                    {
                                        type = "string",
                                        description = "A short explanation (2-4 sentences) explaining the assigned bias score."
                                    }
                                },
                                required = new[] { "Title", "Detail", "BiasScore", "BiasScoreExplanation" }
                            }
                        }
                    },
                    temperature = 1,
                    max_completion_tokens = 10000,
                    top_p = 1,
                    frequency_penalty = 0,
                    presence_penalty = 0,
                    store = true
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

                var response = await _httpClient.SendAsync(request, cancellationToken);

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
        public async Task<QuestionsAndAnswersDto> SendNewsQuestionsAndAnswersPrompt(string detailOfNew, LanguageEnums language, CancellationToken cancellationToken)
        {
            var prompt = string.Empty;
            switch (language)
            {
                case LanguageEnums.TR:
                    prompt = await NewsQuestionsAndAnswersPromptAsTurskish(detailOfNew);
                    break;
                case LanguageEnums.EN:
                    prompt = await NewsQuestionsAndAnswersPrompt(detailOfNew);
                    break;
                default:
                    break;
            }
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
        /// Sends a prompt to the GPT API to generate content based on the provided prompt and language.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="language"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<HttpResponseMessage> SendGeneratedContentPromptAndGetResponse(string prompt, LanguageEnums language, CancellationToken cancellationToken)
        {
            try
            {
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;

                var requestData = new
                {
                    model = "gpt-4o-mini",
                    messages = new object[]
                    {
                new
                {
                    role = "system",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text =await ContentGeneratePromptMessageHeading(language)
                        }
                    }
                },
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = prompt
                        }
                    }
                }
                    },
                    response_format = new
                    {
                        type = "json_object"
                    },
                    temperature = 1,
                    max_completion_tokens = 10000,
                    top_p = 1,
                    frequency_penalty = 0,
                    presence_penalty = 0
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
        /// Sends a request to the GPT API to generate new content subheadings based on the provided base title.
        /// </summary>
        /// <param name="ContentBaseTitle"></param>
        /// <param name="language"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        public async Task<HttpResponseMessage> SendForGetNewContentSubheadingsPromptAndGetResponse(string ContentBaseTitle, LanguageEnums language, CancellationToken cancellationToken)
        {
            try
            {
                var url = _configuration.GetSection("Urls:GptApi").Value;
                var apiKey = _configuration.GetSection("Keys:GptApiKey").Value;

                var requestData = new
                {
                    model = "gpt-4o-mini",
                    messages = new object[]
                    {
                new
                {
                    role = "developer",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = await GenerateSubheadingsForNewContents(ContentBaseTitle,language)
                        }
                    }
                },
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = ContentBaseTitle
                        }
                    }
                }
                    },
                    response_format = new
                    {
                        type = "json_schema",
                        json_schema = new
                        {
                            name = "contentTitles",
                            strict = false,
                            schema = new
                            {
                                type = "object",
                                properties = new
                                {
                                    titles = new
                                    {
                                        type = "array",
                                        items = new
                                        {
                                            type = "string"
                                        },
                                        minItems = 30,
                                        maxItems = 30
                                    }
                                },
                                required = new[] { "Titles" }
                            }
                        }
                    },
                    reasoning_effort = "medium",
                    store = true
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
        /// Generates a prompt message for the GPT API to create content based on the provided language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private static async Task<string> ContentGeneratePromptMessageHeading(LanguageEnums language)
        {
            switch (language)
            {
                case LanguageEnums.TR:
                    await Task.CompletedTask;
                    return @"Lütfen bu başlığa uygun olarak:
                                - 8000-1000 kelimelik açıklayıcı, sade ve öğretici bir içerik yaz, step step belirt (Türkçe).
                                - Görsel üretimi için DALL·E’ye uygun yaratıcı bir görsel betimleme prompt’u oluştur.
                                - 2 adet önemli soru ve cevap üret.
                                - 3 adet sosyal medya etiketi üret (#etiket formatında).

                                Sonucu şu JSON formatında döndür:
                                {
                                  ""SubTitle"": ""..."",
                                  ""Steps"": [{""StepNumber"":""..."",""StepTitle"":""..."", ""StepDescription"":""...""}],
                                  ""ImagePrompt"": ""..."",
                                  ""Questions"": [
                                    { ""Question"": ""..."", ""Answer"": ""..."" },
                                    { ""Question"": ""..."", ""Answer"": ""..."" }
                                  ],
                                  ""Hashtags"": ""...""
                                }";
                case LanguageEnums.EN:

                    await Task.CompletedTask;
                    return @"Please write a detailed, simple, and educational content of 8000-10000 words suitable for this title, specifying step by step (in English).
                                - Create a creative visual description prompt suitable for DALL·E for image production.
                                - Generate 2 important questions and answers.
                                - Generate 3 social media hashtags (#hashtag format).
                                Return the result in the following JSON format:
                                {
                                  ""SubTitle"": ""..."",
                                  ""Steps"": [{""StepNumber"":""..."",""StepTitle"":""..."", ""StepDescription"":""...""}],
                                  ""ImagePrompt"": ""..."",
                                  ""Questions"": [
                                    { ""Question"": ""..."", ""Answer"": ""..."" },
                                    { ""Question"": ""..."", ""Answer"": ""..."" }
                                  ],
                                  ""Hashtags"": ""...""
                                }";
                default:

                    await Task.CompletedTask;
                    return string.Empty;
            }
        }

        /// <summary>
        /// Generates a prompt message for the GPT API to create a horoscope based on the provided horoscope sign.
        /// </summary>
        /// <param name="ContentBaseTitle"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private static async Task<string> GenerateSubheadingsForNewContents(string ContentBaseTitle, LanguageEnums language)
        {
            await Task.CompletedTask;
            var random = new Random();
            var seedValue = random.Next(1000, 9999);
            switch (language)
            {
                case LanguageEnums.TR:
                    return @$"
                            Aşağıdaki kurallara uygun şekilde içerik başlıkları üret:

                            - Sana 1 kategori veriyorum:
                              {ContentBaseTitle}

                            - Her çalıştırıldığında bu kategoriden 30 adet içerik başlığı üret.

                            Kurallar:
                            - Üretilen başlıklar farklı, özgün ve çeşitlendirilmiş olsun.
                            - Başlıklar kısa, net ve araştırmaya uygun olsun (gereksiz açıklama yapma, sadece başlığı ver).
                            - Başlıklar önceden üretilenlerle birebir aynı olmamalı (mümkün olduğunca yeni varyasyonlar bul).
                            - Aynı konsepti tekrar etmekten kaçın.
                            - Gerekirse yeni alanlardan esinlen (bilim, tarih, sağlık, teknoloji, doğaüstü vs.).
                            - Üretim mantığı rastlantısal olsun: bazı başlıklar bilimsel, bazıları ilginç, bazıları günlük hayata yakın olabilir.
                            - Sadece saf başlık listesi ver, açıklama veya ek bilgi ekleme.

                            SeedValue = {seedValue}
                            ";

                case LanguageEnums.EN:
                    return @$"
                        Generate content titles according to the following rules:

                        - You are given 1 category:
                          {ContentBaseTitle}

                        - Each time you run this, generate 30 content titles for the given category.

                        Rules:
                        - The generated titles must be different, original, and varied.
                        - Titles should be short, clear, and research-friendly (do not add unnecessary explanations, only provide the title).
                        - Titles must not be exactly the same as previously generated ones (generate new variations as much as possible).
                        - Avoid repeating the same concepts.
                        - Feel free to draw inspiration from new areas (science, history, health, technology, supernatural, etc.).
                        - The generation should be random: some titles can be scientific, some interesting, and some related to everyday life.
                        - Provide only a pure list of titles, without any explanations or additional information.

                        SeedValue = {seedValue}
                        ";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Sends the provided prompt to the GPT API and returns the response.
        /// </summary>
        /// <param name="prompt">The prompt to send to the GPT API.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>F
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
        /// Creates a Turkish prompt message formatted for OpenAI API call.
        /// </summary>
        /// <param name="detailOfNews">The news content to process.</param>
        /// <returns>A formatted prompt string in Turkish.</returns>
        private async Task<string> TurkishPromptMessage(string detailOfNews)
        {
            var prompt = $@"
                        Sen profesyonel bir gazeteci yapay zekâsısın.

                        Görevin:
                        - Aşağıda verilen haber içeriğini oku ve derinlemesine analiz et: '{detailOfNews}'
                        - Haberi kendi yorumun ve analizlerinle baştan yaz. Orijinal haberin ana fikrine sadık kalarak, içeriği yeniden düzenle ve geliştir.
                        - Yazım kurallarına, dil bilgisine ve anlam bütünlüğüne dikkat ederek haberi akıcı, profesyonel ve etkileyici bir dille kaleme al.

                        Haber Yazım Kuralları:
                        - Kaynak adı veya bağlantısı (link) verme.
                        - Sadece haberin ana içeriğini ve kendi yorumunu kullanarak haber üret.
                        - Haber yazısında aşırı öznel yorumlardan kaçın. Tarafsızlık ve nesnellik çerçevesinde yorum yap.
                        - Okuyucunun kolayca anlayabileceği, açık ve net bir dil kullan.

                        Formatlama Kuralları:
                        - Haber metninde okunabilirliği artırmak için HTML etiketleri kullan:
                          - <h2> başlık için
                          - <p> paragraflar için
                          - <ul> ve <li> listeler için
                          - <strong> veya <em> önemli ifadeler için

                        Haber Uzunluğu:
                        - Yazı mümkün olduğunca uzun, detaylı ve okuyucuyu bilgilendirici olmalıdır.

                        Taraflılık Değerlendirmesi:
                        - Yazının sonunda, haberin genel taraflılık düzeyini değerlendir.
                        - BiasScore ver: 0 (tarafsız) ile 100 (aşırı taraflı) arasında bir değer.
                        - BiasScoreExplanation: Bu puanı neden verdiğini açıklayan 2-4 cümlelik kısa bir yorum yaz.

                        Yanıt Formatı:
                        - Sadece aşağıdaki JSON formatında yanıt ver.
                        - Kesinlikle kod bloğu (```json gibi) kullanma.

                        Beklenen JSON Yapısı:

                        {{
                          ""Title"": ""[Kısa ve etkileyici haber başlığı]"",
                          ""Detail"": ""[HTML etiketleri kullanılarak yazılmış detaylı haber metni]"",
                          ""BiasScore"": ""[0-100 arasında tam sayı]"",
                          ""BiasScoreExplanation"": ""[Taraflılık puanı açıklaması]"",
                          ""ImagePrompt"": ""[Fotoğraf oluşturmak için prompt]""
                        }}
                        ";

            return await Task.FromResult(prompt.Trim());
        }

        /// <summary>
        /// Creates an English prompt message formatted for OpenAI API call.
        /// </summary>
        /// <param name="detailOfNews">The news content to process.</param>
        /// <returns>A formatted prompt string in English.</returns>
        private async Task<string> EnglishPromptMessage(string detailOfNews)
        {
            var prompt = $@"
                        You are a professional journalist AI.

                        Your task:
                        - Read and deeply analyze the following news content: '{detailOfNews}'
                        - Rewrite the article using your own commentary and analysis. Preserve the main idea but enhance the clarity and depth.
                        - Ensure grammatical accuracy, flow, and professional tone throughout the writing.

                        Article Writing Rules:
                        - Do not cite sources, links, or external references.
                        - Only use the core news content and your own commentary.
                        - Avoid overly subjective opinions. Maintain objectivity and journalistic neutrality.
                        - Use a clear, easy-to-read style for general audiences.

                        Formatting Rules:
                        - Use proper HTML tags for better readability:
                          - <h2> for major headings
                          - <p> for paragraphs
                          - <ul> and <li> for listing important points
                          - <strong> or <em> for emphasis when needed

                        Article Length:
                        - Write as long and detailed as possible, engaging the reader.

                        Bias Evaluation:
                        - After the article, evaluate the overall bias.
                        - Provide a BiasScore between 0 (unbiased) and 100 (extremely biased).
                        - Provide a short BiasScoreExplanation explaining the reason for the score in 2-4 sentences.

                        Response Format:
                        - Respond only in the following pure JSON structure.
                        - Do not wrap with any markdown (such as ```json).

                        Expected JSON Format:

                        {{
                          ""Title"": ""[Short and engaging news title]"",
                          ""Detail"": ""[Formatted article with HTML tags]"",
                          ""BiasScore"": ""[Bias score 0-100]"",
                          ""BiasScoreExplanation"": ""[Brief explanation for the bias score]"",
                          ""ImagePrompt"": ""[Prompt to create photo]""
                        }}
                        ";

            return await Task.FromResult(prompt.Trim());
        }

        /// <summary>
        /// Creates a prompt message for generating horoscope content.
        /// </summary>
        /// <param name="horoscope">The name of the horoscope sign.</param>
        /// <returns>A string containing the horoscope prompt message.</returns>
        private async Task<string> HoroscopePromptMessage(string horoscope)
        {
            var prompt = $@"Bir astroloji uzmanı gibi {DateTime.UtcNow.ToString("dd/MM/yyyy").ToString()} tarihi için günlük {horoscope} burcunun yorumunu yap. sadece cevabını yaz, açıklayıcı ve orta uzunlukta yaz.Profesyonel türkçe kullan.";


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

        /// <summary>
        /// Creates a prompt message for generating questions and answers related to news content.
        /// </summary>
        /// <param name="detailOfNew">The detailed content of the news to generate questions and answers for.</param>
        /// <returns>A string containing the questions and answers prompt message.</returns>
        private async Task<string> NewsQuestionsAndAnswersPromptAsTurskish(string detailOfNew)
        {
            var prompt = $@"'{detailOfNew}' Bu haberi okuyup sorgulamamız gereken 3 ila 5 adet soru oluşturabilir misin? Soruları 'questions' başlığı altında JSON formatında toplayabilir misin? Ve bu soruların yanıtlarını da bana JSON olarak [questions{{ [question:'', answer:''] }}] formatında verebilir misin? Ben bu verileri API üzerinden okuyacağım. ÖNEMLİ: Yanıtı sadece metin olarak, JSON formatına dönüştürülmüş şekilde ver.";

            return await Task.FromResult(prompt);
        }
    }
}

