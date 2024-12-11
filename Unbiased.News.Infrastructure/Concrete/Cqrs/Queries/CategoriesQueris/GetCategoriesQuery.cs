using MediatR;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Cqrs.Queries.Categories
{
    public record GetCategoriesQuery():IRequest<List<Category>>;
}

