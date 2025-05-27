using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Engine;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Engine
{
    /// <summary>
    /// Handler for processing GetAllEngineConfigurationsQuery requests to retrieve all engine configurations.
    /// </summary>
    public class GetAllEngineConfigurationsHandler : IRequestHandler<GetAllEngineConfigurationsQuery, IEnumerable<EngineConfigurationDto>>
    {
        private readonly IEngineRepository _engineRepository;

        /// <summary>
        /// Initializes a new instance of the GetAllEngineConfigurationsHandler class.
        /// </summary>
        /// <param name="engineRepository">The engine repository for data access operations.</param>
        public GetAllEngineConfigurationsHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        /// <summary>
        /// Handles the GetAllEngineConfigurationsQuery request and returns a collection of all engine configurations.
        /// </summary>
        /// <param name="request">The query request for retrieving all engine configurations.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation containing a collection of engine configuration DTOs.</returns>
        public async Task<IEnumerable<EngineConfigurationDto>> Handle(GetAllEngineConfigurationsQuery request, CancellationToken cancellationToken)
        {
            return await _engineRepository.GetAllEngineConfigurationsAsync();
        }
    }
}
