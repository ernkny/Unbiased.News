using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving random recent news items grouped by categories for the home page.
    /// </summary>
    public class GetHomePageCategoriesRandomLastGeneratedNewsHandler : IRequestHandler<GetHomePageCategoriesRandomLastGeneratedNewsQuery, IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHomePageCategoriesRandomLastGeneratedNewsHandler"/> class.
        /// </summary>
        /// <param name="categoriesRepository">The categories repository for accessing data.</param>
        public GetHomePageCategoriesRandomLastGeneratedNewsHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        /// <summary>
        /// Handles the request to retrieve random recent news items grouped by categories for the home page.
        /// </summary>
        /// <param name="request">The query request containing language information.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of categories with their random recent news items.</returns>
        public async Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> Handle(GetHomePageCategoriesRandomLastGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            return await _categoriesRepository.GetHomePageCategoriesRandomLastGeneratedNewsAsync(request.language);
        }
    }
}
