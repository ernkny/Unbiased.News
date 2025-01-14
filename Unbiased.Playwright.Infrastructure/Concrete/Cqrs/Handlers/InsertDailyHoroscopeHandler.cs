using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class InsertDailyHoroscopeHandler : IRequestHandler<InsertDailyHoroscopeCommand, bool>
    {
        private readonly IContentRepository _contentRepository;

        public InsertDailyHoroscopeHandler(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public async Task<bool> Handle(InsertDailyHoroscopeCommand request, CancellationToken cancellationToken)
        {
            var result= await _contentRepository.AddDailyHoroscopeAsync(request.horoscopeDetail);
            return result;
        }
    }
}
