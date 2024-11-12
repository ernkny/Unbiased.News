using MediatR;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    public class AddNewsQueryHandler : IRequestHandler<AddNewsCommand, Guid>
    {
        private readonly INewsRepository _newsRepository;

        public AddNewsQueryHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<Guid> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            return await _newsRepository.AddNewsAsync(request.addNewsDto);
        }
    }
}
