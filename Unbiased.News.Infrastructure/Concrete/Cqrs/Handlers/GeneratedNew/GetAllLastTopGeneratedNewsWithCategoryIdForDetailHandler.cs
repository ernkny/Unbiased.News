using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    public class GetAllLastTopGeneratedNewsWithCategoryIdForDetailHandler : IRequestHandler<GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery, IEnumerable<GenerateNewsWithImageDto>>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllLastTopGeneratedNewsWithCategoryIdForDetailHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<GenerateNewsWithImageDto>> Handle(GetAllLastTopGeneratedNewsWithCategoryIdForDetailQuery request, CancellationToken cancellationToken)
        {
            var result = await _newsRepository.GetAllLastTopGeneratedNewsWithCategoryIdForDetailAsync(request.categoryId, request.id,request.language);
            return result;
        }
    }
}
