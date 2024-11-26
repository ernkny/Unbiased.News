using MediatR;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class GetAllNewsCombinedDetailHandler : IRequestHandler<GetAllNewsCombinedDetailsQuery, IEnumerable<GeneratedNewsDto>>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllNewsCombinedDetailHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<GeneratedNewsDto>> Handle(GetAllNewsCombinedDetailsQuery request, CancellationToken cancellationToken)
        {
           return await _newsRepository.GetAllCombinedDetailsAsync();       
        }
    }
}
