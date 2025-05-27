using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    /// <summary>
    /// Handler for processing InsertGeneratedNewsWithImageCommand requests to create generated news with image.
    /// </summary>
    public class InsertGeneratedNewsWithImageHandler : IRequestHandler<InsertGeneratedNewsWithImageCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the InsertGeneratedNewsWithImageHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository for data access operations.</param>
        public InsertGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the InsertGeneratedNewsWithImageCommand request and creates generated news with image.
        /// </summary>
        /// <param name="request">The command request containing the news with image data.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(InsertGeneratedNewsWithImageCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.InsertGeneratedNewsWithImageAsync(request.newsWithImageDto);
        }
    }
}
