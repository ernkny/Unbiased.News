using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.Category;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNew
{
    public class GetHomePageCategorieSliderWithCountHandler : IRequestHandler<GetHomePageCategorieSliderWithCountQuery, IEnumerable<HomePageCategorieSliderWithCountDto>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public GetHomePageCategorieSliderWithCountHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<HomePageCategorieSliderWithCountDto>> Handle(GetHomePageCategorieSliderWithCountQuery request, CancellationToken cancellationToken)
        {
            return await _categoriesRepository.GetHomePageCategorieSliderWithCountAsync();
        }
    }
}
