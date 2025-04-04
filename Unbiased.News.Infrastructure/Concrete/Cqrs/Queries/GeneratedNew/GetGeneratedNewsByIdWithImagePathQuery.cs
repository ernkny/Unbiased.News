using MediatR;
using Unbiased.News.Domain.DTOs;
using Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNew
{
    /// <summary>
    /// Represents a query to retrieve a specific news item by its ID with associated image.
    /// </summary>
    /// <param name="id">The unique identifier of the news item to retrieve.</param>
    public record GetGeneratedNewsByIdWithImagePathQuery(string id) : IRequest<GenerateNewsWithImageDto>;
}
