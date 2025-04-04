using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Category;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving home page category slider data with news counts.
    /// </summary>
    public class GetHomePageCategorieSliderWithCountHandler : IRequestHandler<GetHomePageCategorieSliderWithCountQuery, IEnumerable<HomePageCategorieSliderWithCountDto>>
    {
        private readonly ICategoriesRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHomePageCategorieSliderWithCountHandler"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository for accessing data.</param>
        public GetHomePageCategorieSliderWithCountHandler(ICategoriesRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handles the request to retrieve home page category slider data with news counts.
        /// </summary>
        /// <param name="request">The query request containing language information.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of category slider data with news counts.</returns>
        public async Task<IEnumerable<HomePageCategorieSliderWithCountDto>> Handle(GetHomePageCategorieSliderWithCountQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetHomePageCategorieSliderWithCountAsync(request.language);
        }
    }
}
