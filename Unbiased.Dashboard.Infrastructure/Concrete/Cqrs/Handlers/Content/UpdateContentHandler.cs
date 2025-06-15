using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Content;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Content
{
    /// <summary>
    ///  Handles the command to update content data in the system.
    /// </summary>
    public class UpdateContentHandler : IRequestHandler<UpdateContentCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        ///  Initializes a new instance of the UpdateContentHandler class.
        /// </summary>
        /// <param name="contentRepository"></param>
        public UpdateContentHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        ///  Handles the request to update content data asynchronously.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
        {
            return await _contentRepository.UpdateGeneratedContentAsync(request.UpdateAllContentDataRequest);
        }
    }
}
