using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    /// <summary>
    /// Handler for retrieving a specific news item by its ID with associated image.
    /// </summary>
    public class GetGeneratedNewsByIdWithImagePathHandler : IRequestHandler<GetGeneratedNewsByIdWithImagePathQuery, GenerateNewsWithImageDto>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetGeneratedNewsByIdWithImagePathHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The news repository for accessing data.</param>
        public GetGeneratedNewsByIdWithImagePathHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the request to retrieve a specific news item by its ID with associated image.
        /// </summary>
        /// <param name="request">The query request containing the ID of the news item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested news item with its associated image.</returns>
        public async Task<GenerateNewsWithImageDto> Handle(GetGeneratedNewsByIdWithImagePathQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetGeneratedNewsByIdWithImageAsync(request.id);
        }
    }
}
