using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Engine
{
    /// <summary>
    /// Handler for processing ActivateEngineImmediatlyCommand requests to activate the engine immediately.
    /// </summary>
    public class ActivateEngineImmediatlyHandler : IRequestHandler<ActivateEngineImmediatlyCommand, bool>
    {
        private readonly IEngineRepository _engineRepository;

        /// <summary>
        /// Initializes a new instance of the ActivateEngineImmediatlyHandler class.
        /// </summary>
        /// <param name="engineRepository">The engine repository for data access operations.</param>
        public ActivateEngineImmediatlyHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        /// <summary>
        /// Handles the ActivateEngineImmediatlyCommand request and activates the specified engine immediately.
        /// </summary>
        /// <param name="request">The command request containing the engine ID to activate.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a boolean indicating success.</returns>
        public async Task<bool> Handle(ActivateEngineImmediatlyCommand request, CancellationToken cancellationToken)
        {
            return await _engineRepository.ActivateEngineImmediatlyAsync(request.id);
        }
    }
}
