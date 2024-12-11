using MediatR;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Cqrs.Queries.Categories;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Handlers.Categories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public GetCategoriesQueryHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoriesRepository.GetAllCategoriesAsync();
            return result.ToList();
        }
    }
}
