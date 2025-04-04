using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving the latest top news items from a specific category for detail pages.
    /// </summary>
    public class GetAllLastTopGeneratedNewsWithCategoryIdForDetailHandler : IRequestHandler<GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllLastTopGeneratedNewsWithCategoryIdForDetailHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetAllLastTopGeneratedNewsWithCategoryIdForDetailHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to retrieve the latest top news items from a specific category,
        /// excluding the news item with the provided unique URL.
        /// </summary>
        /// <param name="request">The query request containing category ID, unique URL path, and language.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of the latest top news items from the specified category.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(request.CategoryId, request.Id, request.Language);
        }
    }
}
