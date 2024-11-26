using MediatR;
using Unbiased.Playwright.Domain.Entities;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class GetAllNewsByNotIncludedProcessHandler : IRequestHandler<GetAllNewsByNotIncludedProcessQuery, IEnumerable<News>>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllNewsByNotIncludedProcessHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<News>> Handle(GetAllNewsByNotIncludedProcessQuery request, CancellationToken cancellationToken)
        {
            var result = await _newsRepository.GetAllNewsByNotIncludedProcessAsync();
            return result;
        }
    }
}
