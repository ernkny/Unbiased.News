using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the AddRangeAllNewsCommand by adding a range of news items to the repository.
    /// </summary>
    public class AddRangeAllNewsHandler : IRequestHandler<AddRangeAllNewsCommand, bool>
    {
        /// <summary>
        /// The news repository instance used to perform the operation.
        /// </summary>
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the AddRangeAllNewsHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for adding the range of news items.</param>
        public AddRangeAllNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the AddRangeAllNewsCommand by adding a range of news items to the repository.
        /// </summary>
        /// <param name="request">The AddRangeAllNewsCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a boolean indicating success.</returns>
        public async Task<bool> Handle(AddRangeAllNewsCommand request, CancellationToken cancellationToken)
        {
            var result = await _newsRepository.AddRangeAllNewsAsync(request.listOfNews);
            return result;
        }
    }
}
