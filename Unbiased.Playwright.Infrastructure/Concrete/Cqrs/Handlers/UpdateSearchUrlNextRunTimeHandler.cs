using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class UpdateSearchUrlNextRunTimeHandler : IRequestHandler<UpdateSearchUrlNextRunTimeCommand,bool>
    {
        private readonly ISearchUrlRepository _searhcUrlRepository;

        public UpdateSearchUrlNextRunTimeHandler(ISearchUrlRepository searhcUrlRepository)
        {
            _searhcUrlRepository = searhcUrlRepository;
        }

        public async Task<bool> Handle(UpdateSearchUrlNextRunTimeCommand request, CancellationToken cancellationToken)
        {
           return  await _searhcUrlRepository.UpdateAllSearhcUrlNextRunTimeAsync();
        }
    }
}
