using MediatR;
using Unbiased.Playwright.Domain.DTOs;
using Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Queries;
using Unbiased.Playwright.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Playwright.Infrastructure.Concrete.Cqrs.Handlers
{
    public class GetImagesWithNoneHasGeneratedHandlers : IRequestHandler<GetImagesWithNoneHasGeneratedQuery, IEnumerable<GeneratedNewsWithNoneImageDto>>
    {
        private readonly INewsImageRepository _newsImageRepository;

        public GetImagesWithNoneHasGeneratedHandlers(INewsImageRepository newsImageRepository)
        {
            _newsImageRepository = newsImageRepository;
        }

        public async Task<IEnumerable<GeneratedNewsWithNoneImageDto>> Handle(GetImagesWithNoneHasGeneratedQuery request, CancellationToken cancellationToken)
        {
            return await _newsImageRepository.GenerateImagesWithNoneHasGeneratedAsync(cancellationToken);
        }
    }
}
