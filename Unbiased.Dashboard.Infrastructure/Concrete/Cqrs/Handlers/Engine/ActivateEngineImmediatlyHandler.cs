using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Engine
{
    public class ActivateEngineImmediatlyHandler : IRequestHandler<ActivateEngineImmediatlyCommand, bool>
    {
        private readonly IEngineRepository _engineRepository;

        public ActivateEngineImmediatlyHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        public async Task<bool> Handle(ActivateEngineImmediatlyCommand request, CancellationToken cancellationToken)
        {
            return await _engineRepository.ActivateEngineImmediatlyAsync(request.id);
        }
    }
}
