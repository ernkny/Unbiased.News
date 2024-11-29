using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    /// <summary>
    /// Handles the AddGeneratedNewsCommand by adding the generated news to the repository.
    /// </summary>
    public class AddGeneratedNewsHandler : IRequestHandler<AddGeneratedNewsCommand, bool>
    {
        // Repository for news operations
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the AddGeneratedNewsHandler class.
        /// </summary>
        /// <param name="newsRepository">The repository for news operations.</param>
        public AddGeneratedNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the AddGeneratedNewsCommand by adding the generated news to the repository.
        /// </summary>
        /// <param name="request">The command to add generated news.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> Handle(AddGeneratedNewsCommand request, CancellationToken cancellationToken)
        {
            // Add the generated news to the repository
            return await _newsRepository.AddGeneratedNews(request.News);
        }
    }
}
