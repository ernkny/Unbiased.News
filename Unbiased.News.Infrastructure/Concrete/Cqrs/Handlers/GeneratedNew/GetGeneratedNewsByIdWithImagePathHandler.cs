using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
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
