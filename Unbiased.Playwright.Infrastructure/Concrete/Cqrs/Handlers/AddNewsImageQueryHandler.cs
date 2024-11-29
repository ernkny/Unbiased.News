using MediatR;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    /// <summary>
    /// Handles the AddNewsImageCommand by adding a new news image to the repository.
    /// </summary>
    public class AddNewsImageQueryHandler : IRequestHandler<AddNewsImageCommand, Guid>
    {
        /// <summary>
        /// The repository for news images.
        /// </summary>
        private readonly INewsImageRepository _newsImageRepository;

        /// <summary>
        /// Initializes a new instance of the AddNewsImageQueryHandler class.
        /// </summary>
        /// <param name="newsImageRepository">The repository for news images.</param>
        public AddNewsImageQueryHandler(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        /// <summary>
        /// Handles the AddNewsImageCommand by adding a new news image to the repository.
        /// </summary>
        /// <param name="request">The AddNewsImageCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The ID of the newly added news image.</returns>
        public async Task<Guid> Handle(AddNewsImageCommand request, CancellationToken cancellationToken)
        {
            // Add the news image to the repository and return the ID
            return await _newsImageRepository.AddNewsImageAsync(request.addNewsImageDto);
        }
    }
}
