using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    /// <summary>
    /// Handler for processing GetGeneratedNewsQuery requests to retrieve generated news with images.
    /// </summary>
    public class GetGeneratedNewsHandler : IRequestHandler<GetGeneratedNewsQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the GetGeneratedNewsHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository for data access operations.</param>
        public GetGeneratedNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetGeneratedNewsQuery request and returns a collection of generated news with images.
        /// </summary>
        /// <param name="request">The query request containing parameters for retrieving generated news.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of generated news with image DTOs.</returns>
        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageAsync(request.GetGeneratedNewsWithImagePathRequestDto);
        }
    }
}
