using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.News.Application.Cqrs.Queries.Categories;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.DataAccess.Repositories.Abstract;

namespace Unbiased.News.Application.Cqrs.Handlers.Categories
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
            var result=await _categoriesRepository.GetAllCategoriesAsync();
            return result.ToList();
        }
    }
}
