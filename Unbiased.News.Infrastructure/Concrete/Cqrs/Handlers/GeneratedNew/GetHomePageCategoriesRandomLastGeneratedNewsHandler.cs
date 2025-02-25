using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    public class GetHomePageCategoriesRandomLastGeneratedNewsHandler : IRequestHandler<GetHomePageCategoriesRandomLastGeneratedNewsQuery, IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public GetHomePageCategoriesRandomLastGeneratedNewsHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> Handle(GetHomePageCategoriesRandomLastGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoriesRepository.GetHomePageCategoriesRandomLastGeneratedNewsAsync();
            return result;
        }
    }
}
