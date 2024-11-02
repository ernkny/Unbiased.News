using MediatR;
using Unbiased.Playwright.Application.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    public class InsertNewsQueryHandler : IRequestHandler<InsertNewsQuery, Guid>
    {
        private readonly INewsRepository _newsRepository;

        public InsertNewsQueryHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Guid> Handle(InsertNewsQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.InsertNewsAsync(request.addNewsDto);
        }
    }
}
