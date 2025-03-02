using MediatR;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Commands.Engine;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Engine
{
    public class DeActivateOrActivateSearchHandler : IRequestHandler<DeActivateOrActivateSearchCommand, bool>
    {
        private readonly IEngineRepository _engineRepository;

        public DeActivateOrActivateSearchHandler(IEngineRepository engineRepository)
        {
            _engineRepository = engineRepository;
        }

        public async Task<bool> Handle(DeActivateOrActivateSearchCommand request, CancellationToken cancellationToken)
        {
            return await _engineRepository.DeActivateOrActivateSearchAsync(request.id);
        }
    }
}
