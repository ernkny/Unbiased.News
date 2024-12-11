using MediatR;
using Unbiased.News.Application.Interfaces;
using Unbiased.News.Domain.Entities;
using Unbiased.News.Infrastructure.Cqrs.Queries.Categories;

namespace Unbiased.News.Application.Services
{
    public sealed class CategoriesService : ICategoriesService
    {
        private readonly IMediator _mediator;

        public CategoriesService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var result=await _mediator.Send(new GetCategoriesQuery());
            return result;
        }
    }
}
