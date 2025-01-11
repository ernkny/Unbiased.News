using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.GeneratedNews
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
