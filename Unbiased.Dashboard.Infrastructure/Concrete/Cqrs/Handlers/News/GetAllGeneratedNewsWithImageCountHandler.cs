using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    /// <summary>
    /// Handler for processing GetAllGeneratedNewsWithImageCountQuery requests to retrieve the total count of generated news with images.
    /// </summary>
    public class GetAllGeneratedNewsWithImageCountHandler : IRequestHandler<GetAllGeneratedNewsWithImageCountQuery, int>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllGeneratedNewsWithImageCountHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository for data access operations.</param>
        public GetAllGeneratedNewsWithImageCountHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetAllGeneratedNewsWithImageCountQuery request and returns the total count of generated news with images.
        /// </summary>
        /// <param name="request">The query request containing parameters for counting generated news with images.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing the total count of generated news with images.</returns>
        public async  Task<int> Handle(GetAllGeneratedNewsWithImageCountQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageCountAsync(request.requestDto);
        }
    }
}
