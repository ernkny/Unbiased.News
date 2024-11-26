using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class AddGeneratedNewsHandler : IRequestHandler<AddGeneratedNewsCommand, bool>
    {
        private readonly INewsRepository _newsRepository;
        public AddGeneratedNewsHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<bool> Handle(AddGeneratedNewsCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.AddGeneratedNews(request.News);
        }
    }
}
