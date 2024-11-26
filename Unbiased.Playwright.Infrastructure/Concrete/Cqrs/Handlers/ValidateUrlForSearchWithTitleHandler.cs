using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{

    public class ValidateUrlForSearchWithTitleHandler : IRequestHandler<ValidateUrlForSearchWithTitleQuery, bool>
    {
        private readonly INewsRepository _newsRepository;
        public ValidateUrlForSearchWithTitleHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Handle(ValidateUrlForSearchWithTitleQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.ValidateUrlForSearchWithTitleAsync(request.title);
        }
    }
}
