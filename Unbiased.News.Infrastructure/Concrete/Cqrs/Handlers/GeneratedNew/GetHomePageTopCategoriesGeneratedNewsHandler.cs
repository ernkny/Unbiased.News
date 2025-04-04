using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving top categories with their generated news for the home page.
    /// </summary>
    public class GetHomePageTopCategoriesGeneratedNewsHandler : IRequestHandler<GetHomePageTopCategoriesGeneratedNewsQuery, IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHomePageTopCategoriesGeneratedNewsHandler"/> class.
        /// </summary>
        /// <param name="categoriesRepository">The categories repository for accessing data.</param>
        public GetHomePageTopCategoriesGeneratedNewsHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        /// <summary>
        /// Handles the request to retrieve top categories with their generated news for the home page.
        /// </summary>
        /// <param name="request">The query request containing language information.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of top categories with their generated news.</returns>
        public async Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> Handle(GetHomePageTopCategoriesGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            return await _categoriesRepository.GetHomePageTopCategoriesGeneratedNewsAsync(request.language);
        }
    }
}
