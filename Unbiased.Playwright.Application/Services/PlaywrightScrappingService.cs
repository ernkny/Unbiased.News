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
    public sealed class PlaywrightScrappingService : IPlaywrightScrappingService
    {
        private readonly IMediator _mediator;

        public PlaywrightScrappingService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<bool> PlaywrightScrappingNewsAndAddRangeNewsAsync()
        {
            var resultOfScrappingNews = await PlaywrightScrappingNewsAsync();
            var result = await SaveAllNewsWithRangeAsync(resultOfScrappingNews);
            return result;
        }
        public async Task<List<News>> PlaywrightScrappingNewsAsync()
        {
            var keywords = await _mediator.Send(new GetAllActiveKeywordsForSearchQuery());
            var listOfNews = new List<News>();
            foreach (var keyword in keywords)
            {
                var searchWithKeywordControl = new GetAllNewsWithUrlAddressFromGoogleControl(keyword);
                var titles = await searchWithKeywordControl.Handle();
                var newsContents = new GetNewsWithGuidControl(titles);
                var news = await newsContents.Handle();
                listOfNews.AddRange(news);
            }
            return listOfNews;
        }

        public async Task<bool> SaveAllNewsWithRangeAsync(List<News> listOfNews)
        {
            var result = await _mediator.Send(new AddRangeAllNewsCommand(listOfNews));
            return await Task.FromResult(true);
        }

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
                    ImageBase64 = imagesForTitles[i]
                }));
            }

            return await Task.FromResult(true);
        }

        public Task<List<News>> PlaywrightScrappingNews()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllNewsWithRange(List<News> listOfNews)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PlaywrightScrappingNewsAndAddRangeNews()
        {
            throw new NotImplementedException();
        }
    }
}
