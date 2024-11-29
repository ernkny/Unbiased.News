using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the UpdateNewsProcessValueAsTrueCommand by updating the news process value to true in the repository.
    /// </summary>
    public class UpdateNewsProcessValueAsTrueHandler : IRequestHandler<UpdateNewsProcessValueAsTrueCommand, bool>
    {
        // The news repository instance used to perform the operation.
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the UpdateNewsProcessValueAsTrueHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for updating the news process value.</param>
        public UpdateNewsProcessValueAsTrueHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the UpdateNewsProcessValueAsTrueCommand by updating the news process value to true in the repository.
        /// </summary>
        /// <param name="request">The UpdateNewsProcessValueAsTrueCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation, returning a boolean indicating success.</returns>
        public async Task<bool> Handle(UpdateNewsProcessValueAsTrueCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.UpdateNewsProcessValueAsTrueAsync(request.matchId);
        }
    }
}
