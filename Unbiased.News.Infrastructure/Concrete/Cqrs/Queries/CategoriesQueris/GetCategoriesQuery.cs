using MediatR;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Cqrs.Queries.Categories
{
    /// <summary>
    /// Represents a query to retrieve a list of categories.
    /// </summary>
    public record GetCategoriesQuery():IRequest<List<Category>>;
}

