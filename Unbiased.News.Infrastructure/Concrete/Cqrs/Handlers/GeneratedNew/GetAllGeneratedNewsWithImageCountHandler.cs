using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving the count of generated news items with images for a specific category.
    /// </summary>
    public class GetAllGeneratedNewsWithImageCountHandler : IRequestHandler<GetAllGeneratedNewsWithImageCountQuery, int>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllGeneratedNewsWithImageCountHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetAllGeneratedNewsWithImageCountHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to count generated news items with images for a specific category.
        /// </summary>
        /// <param name="request">The query request containing category ID and optional title filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The count of news items matching the criteria.</returns>
        public async Task<int> Handle(GetAllGeneratedNewsWithImageCountQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageCountAsync(request.categoryId, request.title);
        }
    }
}
