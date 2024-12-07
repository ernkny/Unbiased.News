using MediatR;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Commands;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class InsertGeneratedImageHandler : IRequestHandler<InsertGeneratedImageCommand, bool>
    {
        private readonly INewsImageRepository _newsImageRepository;

        public InsertGeneratedImageHandler(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        public async Task<bool> Handle(InsertGeneratedImageCommand request, CancellationToken cancellationToken)
        {
            return await _newsImageRepository.AddNewsImageAsync(request.ImageNews);
        }
    }
}
