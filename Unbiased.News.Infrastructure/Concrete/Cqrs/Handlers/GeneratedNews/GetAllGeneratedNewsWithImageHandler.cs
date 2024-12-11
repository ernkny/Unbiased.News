using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNews;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNews
{
    public class GetAllGeneratedNewsWithImageHandler : IRequestHandler<GetAllGeneratedNewsWithImageQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllGeneratedNewsWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetAllGeneratedNewsWithImageQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageAsync();
        }
    }
}
