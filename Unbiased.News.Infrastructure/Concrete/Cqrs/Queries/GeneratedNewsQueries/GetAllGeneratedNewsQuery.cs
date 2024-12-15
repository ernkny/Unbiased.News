using MediatR;
using Entities = Unbiased.News.Domain.Entities;

namespace Unbiased.News.Infrastructure.Concrete.Cqrs.Queries.GeneratedNewsQueries
{
    /// <summary>
    /// Represents a query to retrieve all generated news.
    /// </summary>
    public record GetAllGeneratedNewsQuery() : IRequest<IEnumerable<Entities.GeneratedNews>>;
}
