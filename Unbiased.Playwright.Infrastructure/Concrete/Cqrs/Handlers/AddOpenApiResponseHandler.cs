using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the AddOpenApiResponseCommand by adding the Open AI response to the news repository.
    /// </summary>
    public class AddOpenApiResponseHandler : IRequestHandler<AddOpenApiResponseCommand>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the AddOpenApiResponseHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository to use for adding the Open AI response.</param>
        public AddOpenApiResponseHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the AddOpenApiResponseCommand by adding the Open AI response to the news repository.
        /// </summary>
        /// <param name="request">The AddOpenApiResponseCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(AddOpenApiResponseCommand request, CancellationToken cancellationToken)
        {
            await _newsRepository.AddOpenAiResponseAsync(request.Response);
        }
    }
}
