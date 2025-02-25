using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    internal class GetGeneratedNewsByUniqUrlWithImageHandler : IRequestHandler<GetGeneratedNewsByUniqUrlWithImageQuery, GenerateNewsWithImageDto>
    {
        private readonly INewsRepository _newsRepository;

        public GetGeneratedNewsByUniqUrlWithImageHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<GenerateNewsWithImageDto> Handle(GetGeneratedNewsByUniqUrlWithImageQuery request, CancellationToken cancellationToken)
        {
            var result= await _newsRepository.GetGeneratedNewsByUniqUrlWithImageAsync(request.uniqUrl);
            return result; 
        }
    }
}

