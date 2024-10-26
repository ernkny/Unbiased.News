using MediatR;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Application.Cqrs.Queries.Categories
{
    public record GetCategoriesQuery():IRequest<List<Category>>;
}

