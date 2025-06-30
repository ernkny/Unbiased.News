using MediatR;
using Microsoft.Playwright;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Application.Playwright.Concrete;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Domain.Enums;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Shared.Extensions.Concrete.Entities;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.Playwright.Application.Services
{
    /// <summary>
    /// PlaywrightScrappingService is responsible for handling news scraping and saving operations.
    /// It implements the IPlaywrightScrappingService interface.
    /// </summary>
    public sealed class PlaywrightScrappingService : IPlaywrightScrappingService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAndActivityLog _eventAndActivityLog;
        private readonly IPlaywright _playwright;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the PlaywrightScrappingService class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public PlaywrightScrappingService(IMediator mediator, IServiceProvider serviceProvider, IEventAndActivityLog eventAndActivityLog, IPlaywright playwright)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
            _eventAndActivityLog = eventAndActivityLog;
            _playwright = playwright;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Scrapes news and adds them to the database in a single operation.
        /// </summary>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> PlaywrightScrappingNewsAndAddRangeNewsAsync()
        {
            try
            {
                var urls = await _mediator.Send(new GetAllActiveUrlsForSearchQuery());
                foreach (var url in urls)
                {
                    var resultOfScrappingNews = await PlaywrightScrappingNewsAsync(url);
                    await SaveAllNewsWithRangeAsync(resultOfScrappingNews);
                }
                return Task.CompletedTask.IsCompleted;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }

        /// <summary>
        /// Scrapes news from Google using the provided keywords.
        /// </summary>
        /// <returns>A list of scraped news.</returns>
        public async Task<List<News>> PlaywrightScrappingNewsAsync(ActiveUrlsForSearchDto url)
        {
            try
            {
                var listOfNews = Enumerable.Empty<News>().ToList();
                var languageEnum = (LanguageEnums)Enum.Parse(typeof(LanguageEnums), url.Language);
                var searchWithKeywordControl = new GetAllNewsWithUrlAddressFromGoogleControl(url.url, languageEnum, _serviceProvider, _eventAndActivityLog);
                var titles = await searchWithKeywordControl.Handle();
                var newsContents = new GetNewsWithGuidControl(titles, _serviceProvider, _eventAndActivityLog, _playwright, _httpClient);
                var news = await newsContents.Handle();
                news.ForEach(item => item.CategoryId = url.categoryId);
                news.ForEach(item => item.Language = url.Language);
                listOfNews.AddRange(news);
                return listOfNews;
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }

        /// <summary>
        /// Saves a list of news to the database.
        /// </summary>
        /// <param name="listOfNews">The list of news to save.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> SaveAllNewsWithRangeAsync(List<News> listOfNews)
        {
            try
            {

                var result = await _mediator.Send(new AddRangeAllNewsCommand(listOfNews));
                return await Task.FromResult(true);
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }
        }

        /// <summary>
        /// Gets images for collected news.
        /// </summary>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> GetImagesForCollectedNews()
        {
            try
            {
                var newsWithoutImages = (await _mediator.Send(new GetNewsWithoutImagesQuery(DateTime.Now.AddDays(-100)))).ToList();

                var titles = newsWithoutImages.Select(x => x.Title).ToList();

                var imagesForTitles = await GetImageProcess.GetNewsImageForTitle(titles);

                for (var i = 0; i < imagesForTitles.Count; i++)
                {
                    await _mediator.Send(new AddNewsImageCommand(new InsertNewsImageDto
                    {
                        MatchId = newsWithoutImages[i].MatchId,
                        filePath = imagesForTitles[i]
                    }));
                }

                return await Task.FromResult(true);
            }
            catch (Exception exception)
            {
                await _eventAndActivityLog.SendEventLogToQueue(new EventLog
                {
                    EventType = this.GetType().FullName,
                    EventSeverity = "Error",
                    Message = $"{exception.Message} - {exception.StackTrace}",
                    EventDate = DateTime.UtcNow
                });
                throw;
            }

        }
    }
}
