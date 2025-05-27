using MediatR;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Categories;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Categories
{
    /// <summary>
    /// Handler for processing GetAllCategoriesQuery requests to retrieve all categories.
    /// </summary>
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllCategoriesHandler class.
        /// </summary>
        /// <param name="categoryRepository">The category repository for data access operations.</param>
        public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Handles the GetAllCategoriesQuery request and returns a collection of all categories.
        /// </summary>
        /// <param name="request">The query request for retrieving all categories.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of category entities.</returns>
        public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
    }
}
