using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving banner news items with images for a specific category.
    /// </summary>
    public class GetBannerGeneratedNewsWithImageHandler : IRequestHandler<GetBannerGeneratedNewsWithImageQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBannerGeneratedNewsWithImageHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetBannerGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to retrieve banner news items with images for a specific category and language.
        /// </summary>
        /// <param name="request">The query request containing category ID and language information.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of banner news items with their associated images.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetBannerGeneratedNewsWithImageQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetBannerGeneratedNewsWithImageAsync(request.categoryId, request.language);
        }
    }
}

