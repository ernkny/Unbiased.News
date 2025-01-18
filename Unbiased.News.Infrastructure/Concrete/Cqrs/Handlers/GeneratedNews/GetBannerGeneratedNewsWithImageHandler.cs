using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNews
{
    public class GetBannerGeneratedNewsWithImageHandler : IRequestHandler<GetBannerGeneratedNewsWithImageQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        public GetBannerGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetBannerGeneratedNewsWithImageQuery request, CancellationToken cancellationToken)
        {
            var result =await _newsRepository.GetBannerGeneratedNewsWithImageAsync();
            return result;
        }
    }
}
