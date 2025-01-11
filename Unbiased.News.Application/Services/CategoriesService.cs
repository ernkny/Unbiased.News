using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.CategoriesQueris;
using Unbiased.News.Infrastructure.Cqrs.Queries.Categories;

namespace Unbiased.News.Application.Services
{
    /// <summary>
    /// Service for managing categories.
    /// </summary>
    public sealed class CategoriesService : ICategoriesService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesService"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        public CategoriesService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all categories asynchronously.
        /// </summary>
        /// <returns>A list of categories.</returns>
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var result=await _mediator.Send(new GetCategoriesQuery());
            return result;
        }

        public async Task<List<HomePageCategorieSliderWithCountDto>> GetHomePageCategorieSliderWithCountAsync()
        {
            var result=await _mediator.Send(new GetHomePageCategorieSliderWithCountQuery());
            return result is not null ? result.ToList():Enumerable.Empty<HomePageCategorieSliderWithCountDto>().ToList();
        }
    }
}
