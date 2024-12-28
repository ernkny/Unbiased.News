using MediatR;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Application.Playwright.Concrete;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;

namespace Unbiased.Playwright.Application.Services
{
    /// <summary>
    /// PlaywrightScrappingService is responsible for handling news scraping and saving operations.
    /// It implements the IPlaywrightScrappingService interface.
    /// </summary>
    public sealed class PlaywrightScrappingService : IPlaywrightScrappingService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the PlaywrightScrappingService class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public PlaywrightScrappingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Scrapes news and adds them to the database in a single operation.
        /// </summary>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> PlaywrightScrappingNewsAndAddRangeNewsAsync()
        {
            var resultOfScrappingNews = await PlaywrightScrappingNewsAsync();
            var result = await SaveAllNewsWithRangeAsync(resultOfScrappingNews);
            return result;
        }

        /// <summary>
        /// Scrapes news from Google using the provided keywords.
        /// </summary>
        /// <returns>A list of scraped news.</returns>
        public async Task<List<News>> PlaywrightScrappingNewsAsync()
        {
            var urls = await _mediator.Send(new GetAllActiveUrlsForSearchQuery());
            var listOfNews = new List<News>();
            foreach (var url in urls)
            {
                var searchWithKeywordControl = new GetAllNewsWithUrlAddressFromGoogleControl(url.url);
                var titles = await searchWithKeywordControl.Handle();
                var newsContents = new GetNewsWithGuidControl(titles);
                var news = await newsContents.Handle();
                news.ForEach(item => item.CategoryId = url.categoryId);
                news.ForEach(item => item.Language = url.Language);
                listOfNews.AddRange(news);
            }
            return listOfNews;
        }

        /// <summary>
        /// Saves a list of news to the database.
        /// </summary>
        /// <param name="listOfNews">The list of news to save.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> SaveAllNewsWithRangeAsync(List<News> listOfNews)
        {
            var result = await _mediator.Send(new AddRangeAllNewsCommand(listOfNews));
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Gets images for collected news.
        /// </summary>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> GetImagesForCollectedNews()
        {
            // Get matchId's without images from the database
            var newsWithoutImages = (await _mediator.Send(new GetNewsWithoutImagesQuery(DateTime.Now.AddDays(-100)))).ToList();

            var titles = newsWithoutImages.Select(x => x.Title).ToList();

            var imagesForTitles = await GetImageProcess.GetNewsImageForTitle(titles);

            for ( var i = 0; i < imagesForTitles.Count; i++)
            {
                await _mediator.Send(new AddNewsImageCommand(new InsertNewsImageDto
                {
                    MatchId = newsWithoutImages[i].MatchId,
                    filePath = imagesForTitles[i]
                }));
            }

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Scrapes news (not implemented).
        /// </summary>
        /// <returns>A list of scraped news.</returns>
        public Task<List<News>> PlaywrightScrappingNews()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a list of news to the database (not implemented).
        /// </summary>
        /// <param name="listOfNews">The list of news to save.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public Task<bool> SaveAllNewsWithRange(List<News> listOfNews)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scrapes news and adds them to the database in a single operation (not implemented).
        /// </summary>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public Task<bool> PlaywrightScrappingNewsAndAddRangeNews()
        {
            throw new NotImplementedException();
        }
    }
}
