using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Category;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.Cqrs.Queries.Categories;
using Unbiased.Shared.Extensions.Concrete.Loggging;

namespace Unbiased.News.Application.Services
{
    /// <summary>
    /// Service for managing categories.
    /// </summary>
    public sealed class CategoriesService : ICategoriesService
    {
        private readonly IMediator _mediator;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventAndActivityLog _eventAndActivityLog = new EventAndActivityLog();

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesService"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public CategoriesService(IMediator mediator, IServiceProvider serviceProvider)
        {
            _mediator = mediator;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets all categories asynchronously.
        /// </summary>
        /// <returns>A list of categories.</returns>
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var result = await _mediator.Send(new GetCategoriesQuery());
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<HomePageCategorieSliderWithCountDto>> GetHomePageCategorieSliderWithCountAsync(string language)
        {
            try
            {
                var result = await _mediator.Send(new GetHomePageCategorieSliderWithCountQuery(language));
                return result is not null ? result.ToList() : Enumerable.Empty<HomePageCategorieSliderWithCountDto>().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageCategoriesRandomGeneratedNewsAsync(string language)
        {
            try
            {
                var result = await _mediator.Send(new GetHomePageCategoriesRandomLastGeneratedNewsQuery(language));
                return result is not null ? result.ToList() : Enumerable.Empty<HomePageCategoriesRandomLastGeneratedNewsDto>().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<HomePageCategoriesRandomLastGeneratedNewsDto>> GetHomePageTopCategoriesGeneratedNewsAsync(string language)
        {
            try
            {
                var result = await _mediator.Send(new GetHomePageTopCategoriesGeneratedNewsQuery(language));
                return result is not null ? result.ToList() : Enumerable.Empty<HomePageCategoriesRandomLastGeneratedNewsDto>().ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
