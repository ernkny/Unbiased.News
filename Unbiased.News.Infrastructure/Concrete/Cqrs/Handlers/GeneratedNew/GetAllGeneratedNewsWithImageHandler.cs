using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving generated news items with associated images.
    /// </summary>
    public class GetAllGeneratedNewsWithImageHandler : IRequestHandler<GetAllGeneratedNewsWithImageQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllGeneratedNewsWithImageHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetAllGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to retrieve generated news items with images for a specific category,
        /// with pagination and language filtering support.
        /// </summary>
        /// <param name="request">The query request containing category ID, page number, language, and optional title filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A collection of news items with their associated images.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetAllGeneratedNewsWithImageQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageAsync(request.categoryId, request.pageNumber, request.language, request.title);
        }
    }
}
