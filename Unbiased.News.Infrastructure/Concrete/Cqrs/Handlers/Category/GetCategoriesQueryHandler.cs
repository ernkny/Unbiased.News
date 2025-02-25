using MediatR;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Cqrs.Queries.Categories;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Categories
{
    /// <summary>
    /// Handles the GetCategoriesQuery and returns a list of categories.
    /// </summary>
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        /// <summary>
        /// Initializes a new instance of the GetCategoriesQueryHandler class.
        /// </summary>
        /// <param name="categoriesRepository">The repository for categories.</param>
        public GetCategoriesQueryHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        /// <summary>
        /// Handles the GetCategoriesQuery and returns a list of categories.
        /// </summary>
        /// <param name="request">The GetCategoriesQuery request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of categories.</returns>
        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoriesRepository.GetAllCategoriesAsync();
            return result.ToList();
        }
    }
}
