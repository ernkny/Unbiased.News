using MediatR;
using Unbiased.Dashboard.Domain.Entities;
using Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Queries.Categories;
using Unbiased.Dashboard.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.Dashboard.Infrastructure.Concrete.Cqrs.Handlers.Categories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }
    }
}
