using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class GetAllActiveKeywordsForSearchHandler : IRequestHandler<GetAllActiveKeywordsForSearchQuery, IEnumerable<string>>
    {
        private readonly INewsRepository _newsRepository;
        public GetAllActiveKeywordsForSearchHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<IEnumerable<string>> Handle(GetAllActiveKeywordsForSearchQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllActiveKeywordsForSearchAsync();
        }
    }
}
