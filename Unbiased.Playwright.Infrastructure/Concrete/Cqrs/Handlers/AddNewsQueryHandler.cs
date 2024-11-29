using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.ConcreteInfrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the AddNewsCommand by adding a new news item to the repository.
    /// </summary>
    public class AddNewsQueryHandler : IRequestHandler<AddNewsCommand, Guid>
    {
        /// <summary>
        /// The repository used to store and retrieve news items.
        /// </summary>
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the AddNewsQueryHandler class.
        /// </summary>
        /// <param name="newsRepository">The repository used to store and retrieve news items.</param>
        public AddNewsQueryHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the AddNewsCommand by adding a new news item to the repository.
        /// </summary>
        /// <param name="request">The command containing the news item to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A unique identifier for the newly added news item.</returns>
        public async Task<Guid> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            // Add the news item to the repository and return the generated ID
            return await _newsRepository.AddNewsAsync(request.addNewsDto);
        }
    }
}
