using MediatR;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    public class GetAllGeneratedNewsWithImageCountHandler : IRequestHandler<GetAllGeneratedNewsWithImageCountQuery, int>
    {
        private readonly INewsRepository _newsRepository;

        public GetAllGeneratedNewsWithImageCountHandler(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<int> Handle(GetAllGeneratedNewsWithImageCountQuery request, CancellationToken cancellationToken)
        {
            return await _newsRepository.GetAllGeneratedNewsWithImageCountAsync(request.categoryId,request.title);
        }
    }
}
