using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Application.Interfaces.Playwright;
using Unbiased.Playwright.Application.Playwright.Concrete.Playwright.NewsScrappingProcess;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Services
{
    public sealed class PlaywrightScrappingService : IPlaywrightScrappingService
    {
        private readonly IMediator _mediator;

        public PlaywrightScrappingService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<bool> PlaywrightScrappingNewsAndAddRangeNews()
        {
            var resultOfScrappingNews = await PlaywrightScrappingNews();
            var result = await SaveAllNewsWithRange(resultOfScrappingNews);
            return result;
        }
        public async Task<List<News>> PlaywrightScrappingNews()
        {
            var keywords = await _mediator.Send(new GetAllActiveKeywordsForSearchQuery());
            var listOfNews = Enumerable.Empty<News>().ToList();
            foreach (var keyword in keywords)
            {
                var searchWithKeywordControl = new SearchWithKeywordControl(keyword);
                var titles = await searchWithKeywordControl.Handle();
                var searchWithTitlesControl = new SearchWithTitlesControl(titles);
                await searchWithKeywordControl.SetNext(searchWithTitlesControl);
                var urls = await searchWithTitlesControl.Handle();
                var newsContents = new GetNewsWithGuidControl(urls);
                await searchWithTitlesControl.SetNext(newsContents);
                listOfNews.AddRange(await newsContents.Handle());
            }
            return listOfNews;
        }

        public async Task<bool> SaveAllNewsWithRange(List<News> listOfNews)
        {
            var result = await _mediator.Send(new AddRangeAllNewsCommand(listOfNews));
            return await Task.FromResult(true);
        }
    }
}
