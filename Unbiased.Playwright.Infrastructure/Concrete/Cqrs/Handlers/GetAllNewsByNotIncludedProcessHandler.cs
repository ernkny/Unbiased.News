using MediatR;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the GetAllNewsByNotIncludedProcessQuery by retrieving all news items that have not been included in a process from the repository.
    /// </summary>
    public class GetAllNewsByNotIncludedProcessHandler : IRequestHandler<GetAllNewsByNotIncludedProcessQuery, IEnumerable<News>>
    {
        // The news repository instance used to perform the operation.
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllNewsByNotIncludedProcessHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for retrieving news items.</param>
        public GetAllNewsByNotIncludedProcessHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the GetAllNewsByNotIncludedProcessQuery by retrieving all news items that have not been included in a process from the repository.
        /// </summary>
        /// <param name="request">The GetAllNewsByNotIncludedProcessQuery to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of news items.</returns>
        public async Task<IEnumerable<News>> Handle(GetAllNewsByNotIncludedProcessQuery request, CancellationToken cancellationToken)
        {
            var result = await _newsRepository.GetAllNewsByNotIncludedProcessAsync();
            return result;
        }
    }
}
