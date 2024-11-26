using MediatR;
using Unbiased.Playwright.Application.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Application.Cqrs.Handlers
{
    public class AddNewsImageQueryHandler : IRequestHandler<AddNewsImageCommand, Guid>
    {
        private readonly INewsImageRepository _newsImageRepository;

        public AddNewsImageQueryHandler(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        public async Task<Guid> Handle(AddNewsImageCommand request, CancellationToken cancellationToken)
        {
            return await _newsImageRepository.AddNewsImageAsync(request.addNewsImageDto);
        }
    }
}
