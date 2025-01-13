using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNews
{
    public class GetGeneratedNewsByIdWithImagePathHandler : IRequestHandler<GetGeneratedNewsByIdWithImagePathQuery, GenerateNewsWithImageDto>
    {
        private readonly INewsRepository _newsRepository;
        public GetGeneratedNewsByIdWithImagePathHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<GenerateNewsWithImageDto> Handle(GetGeneratedNewsByIdWithImagePathQuery request, CancellationToken cancellationToken)
        {
            var result= await _newsRepository.GetGeneratedNewsByIdWithImageAsync(request.id);
            return result;
        }
    }
}
