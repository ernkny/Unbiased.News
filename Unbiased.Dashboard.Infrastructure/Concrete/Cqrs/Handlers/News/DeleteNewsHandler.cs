using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    /// <summary>
    /// Handler for processing DeleteGeneretedNewsCommand requests to delete generated news by its identifier.
    /// </summary>
    public class DeleteNewsHandler : IRequestHandler<DeleteGeneretedNewsCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteNewsHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository for data access operations.</param>
        public DeleteNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the DeleteGeneretedNewsCommand request and deletes the specified news.
        /// </summary>
        /// <param name="request">The command request containing the news ID to delete.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(DeleteGeneretedNewsCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.DeleteNewsByIdAsync(request.id);
        }
    }
}
