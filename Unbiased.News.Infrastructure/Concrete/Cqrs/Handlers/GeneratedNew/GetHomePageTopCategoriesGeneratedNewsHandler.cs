using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    public class GetHomePageTopCategoriesGeneratedNewsHandler : IRequestHandler<GetHomePageTopCategoriesGeneratedNewsQuery, IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public GetHomePageTopCategoriesGeneratedNewsHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<HomePageCategoriesRandomLastGeneratedNewsDto>> Handle(GetHomePageTopCategoriesGeneratedNewsQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoriesRepository.GetHomePageTopCategoriesGeneratedNewsAsync();
            return result;
        }
    }
}
