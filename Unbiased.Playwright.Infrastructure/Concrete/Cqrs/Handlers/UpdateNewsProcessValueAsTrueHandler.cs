using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class UpdateNewsProcessValueAsTrueHandler : IRequestHandler<UpdateNewsProcessValueAsTrueCommand, bool>
    {
        private readonly INewsRepository _newsRepository;

        public UpdateNewsProcessValueAsTrueHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Handle(UpdateNewsProcessValueAsTrueCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.UpdateNewsProcessValueAsTrueAsync(request.matchId);
        }
    }
}
