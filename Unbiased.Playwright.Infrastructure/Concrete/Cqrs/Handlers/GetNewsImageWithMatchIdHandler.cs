using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    internal class GetNewsImageWithMatchIdHandler : IRequestHandler<GetNewsImageWithMatchIdQuery, bool>
    {
        private readonly INewsImageRepository   _newsImageRepository;

        public GetNewsImageWithMatchIdHandler(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        public async Task<bool> Handle(GetNewsImageWithMatchIdQuery request, CancellationToken cancellationToken)
        {
            return await _newsImageRepository.GetNewsImageWithMatchIdAsync(request.MatchID);
        }
    }
}
