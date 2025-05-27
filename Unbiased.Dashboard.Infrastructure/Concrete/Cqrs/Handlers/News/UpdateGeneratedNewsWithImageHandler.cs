using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.News;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.News
{
    /// <summary>
    /// Handler for processing UpdateGeneratedNewsWithImageCommand requests to update generated news with image.
    /// </summary>
    public class UpdateGeneratedNewsWithImageHandler : IRequestHandler<UpdateGeneratedNewsWithImageCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        /// <summary>
        /// Initializes a new instance of the UpdateGeneratedNewsWithImageHandler class.
        /// </summary>
        /// <param name="newsRepository">The news repository for data access operations.</param>
        public UpdateGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        /// <summary>
        /// Handles the UpdateGeneratedNewsWithImageCommand request and updates the generated news with image.
        /// </summary>
        /// <param name="request">The command request containing the updated news data.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(UpdateGeneratedNewsWithImageCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.UpdateGeneratedNewsWithImageAsync(request.generatedNewsDto);
        }
    }
}
