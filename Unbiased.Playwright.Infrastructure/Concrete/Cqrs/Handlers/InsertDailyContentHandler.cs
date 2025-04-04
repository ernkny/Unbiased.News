using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class InsertDailyContentHandler : IRequestHandler<InsertDailyContentCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        public InsertDailyContentHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<bool> Handle(InsertDailyContentCommand request, CancellationToken cancellationToken)
        {
            var result= await _contentRepository.AddDailyContentInformationAsync(request.DailyContentDto);
            return result;
        }
    }
}
