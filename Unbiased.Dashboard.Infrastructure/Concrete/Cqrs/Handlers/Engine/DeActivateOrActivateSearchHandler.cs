using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Engine
{
    /// <summary>
    /// Handler for processing DeActivateOrActivateSearchCommand requests to deactivate or activate search functionality.
    /// </summary>
    public class DeActivateOrActivateSearchHandler : IRequestHandler<DeActivateOrActivateSearchCommand, bool>
    {
        private readonly IEngineRepository _engineRepository;

        /// <summary>
        /// Initializes a new instance of the DeActivateOrActivateSearchHandler class.
        /// </summary>
        /// <param name="engineRepository">The engine repository for data access operations.</param>
        public DeActivateOrActivateSearchHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        /// <summary>
        /// Handles the DeActivateOrActivateSearchCommand request and toggles the search functionality state.
        /// </summary>
        /// <param name="request">The command request containing the search configuration ID to toggle.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(DeActivateOrActivateSearchCommand request, CancellationToken cancellationToken)
        {
            return await _engineRepository.DeActivateOrActivateSearchAsync(request.id);
        }
    }
}
