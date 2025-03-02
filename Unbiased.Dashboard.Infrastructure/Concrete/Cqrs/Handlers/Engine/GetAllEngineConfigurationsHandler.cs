using MediatR;
using Unbiased.Dashboard.Domain.Dto_s;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Engine;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Engine
{
    public class GetAllEngineConfigurationsHandler : IRequestHandler<GetAllEngineConfigurationsQuery, IEnumerable<EngineConfigurationDto>>
    {
        private readonly IEngineRepository _engineRepository;

        public GetAllEngineConfigurationsHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        public async Task<IEnumerable<EngineConfigurationDto>> Handle(GetAllEngineConfigurationsQuery request, CancellationToken cancellationToken)
        {
            return await _engineRepository.GetAllEngineConfigurationsAsync();
        }
    }
}
